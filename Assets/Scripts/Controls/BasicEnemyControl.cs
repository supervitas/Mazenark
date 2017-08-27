using System;
using System.Collections.Generic;
using GameEnv.GameEffects;
using GameSystems;
using Items;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Controls {    
    public abstract class BasicEnemyControl : NetworkBehaviour {
        [SerializeField]
        protected Animator Animator;
        protected NavMeshAgent Agent;
        
        [SerializeField]
        protected GameObject Weapon;

        [SerializeField]
        protected float EnemyAgroRange = 20f;
        [SerializeField]
        protected float EnemyAngleVisibility = 60f;

        protected readonly List <Vector3> Points = new List<Vector3>();
        private int _destPoint = 0;                

        public bool IsAlive = true;      

        protected bool CanPatrool = false;        

        protected float RegularSpeed;

        [SerializeField]
        protected float TimeForAttack = 0.5f;
        
        protected float AttackTimePassed = 0f;
        protected float IdleTimePassed = 0f;
        
        [SerializeField]
        protected float SpeedBoostOnAgro = 0.3f;
        
        [SerializeField]
        [Range(3, 40)]
        protected float RangeOfAttack = 6f;
                
        protected Vector3 TargetPosition;
        protected bool WasFolowingPlayer = false;

        private byte _gatherPointsVisited = 0;
        
        private static readonly Vector3 _zeroVector = Vector3.zero;
        private static LayerMask _obstacleMask;       

        protected RaycastHit RaycastHit;

        protected static List<Transform> _playersTransform;       
        
        protected static readonly int _idleAnimation = Animator.StringToHash("Idle");
        protected static readonly int _attackAnimation = Animator.StringToHash("Attack");
        protected static readonly int _movingAnimation = Animator.StringToHash("Moving");
        protected static readonly int _deathAnimation = Animator.StringToHash("isDead");
                        
        public override void OnStartServer() {                    
            if (!isServer) return;
            
            Agent = GetComponent<NavMeshAgent>();
            
            _obstacleMask = LayerMask.GetMask("Obstacles");                     

            if (_playersTransform == null) {
                _playersTransform = FindObjectOfType<GameManager>().GetPlayersTransforms();
            }

            if (Agent != null) {
                RegularSpeed = Agent.speed;
            }

            IsAlive = true;
        }

        protected void SetAnimation(int animation, bool value) {
            if (Animator.GetBool(animation) != value) {
                Animator.SetBool(animation, value);
            }
        }                

        public void AddPatroolPoint(Vector3 patroolPoint) {
            if (!isServer) return;
            
            Points.Add(patroolPoint);                        
        }         

        public void SetPatrool(bool patrool) {
            if (!isServer) return;
            
            CanPatrool = patrool;
        }

        public void Die(float timeOfDeath) {
            if (!isServer) return;

            IsAlive = false;                         
            
            SetAnimation(_idleAnimation, false);
            SetAnimation(_movingAnimation, false);
            
            SetAnimation(_deathAnimation, true);

            if (Agent != null) {
                Agent.isStopped = true;
            }               
        }  

        protected virtual bool CheckPlayersNear(out Vector3 playerTarget) {                        
            foreach (var target in _playersTransform) {
                if (target == null) continue;

                var distance = Vector3.Distance(transform.position, target.position);

                var direction = target.position - transform.position;
                var angle = Vector3.Angle(direction, transform.forward);                
                
                var pos = transform.position;
                var dir = target.position;
                pos.y += 2.5f;
                dir.y += 1.5f;
                                
                Debug.DrawLine(pos, dir);
               
                if ((distance <= EnemyAgroRange && angle < 30 || distance < 10)
                    && !Physics.Linecast(pos, dir, out RaycastHit, _obstacleMask)) {
                                 
                    playerTarget = target.position;
                    
                    return true;
                }
            }
            playerTarget = _zeroVector;           
            return false;
        }

        protected virtual void Update() {            
            if (!isServer || !IsAlive) return;

            Agent.isStopped = false;
            
            SetAnimation(Agent.velocity != _zeroVector ? _movingAnimation : _idleAnimation, true);

            if (CheckPlayersNear(out TargetPosition)) { // Target visible. Need to Attack
                Agent.autoBraking = true;                
                Agent.destination = TargetPosition;
                Agent.isStopped = false; 
                
                WasFolowingPlayer = true;
                
                var direction = Agent.destination - transform.position;

                Agent.speed = RegularSpeed * SpeedBoostOnAgro;
                
                if (direction != _zeroVector) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                }
                
                if (Agent.remainingDistance > RangeOfAttack) {
                    AttackTimePassed = 0;
                    SetAnimation(_attackAnimation, false);
                }
                
                if (CanAttack(TargetPosition)) {
                    Attack();
                }
                return;
            }
            
            if (WasFolowingPlayer) { // Target disapeared. Need to gather              
                Gather();
                return;
            }                                                
                    
            if (CanPatrool && !Agent.pathPending && Agent.remainingDistance <= 0.5f) { // Continue patrool                                
                Agent.destination = Points[_destPoint];
                _destPoint = (_destPoint + 1) % Points.Count;
            }
        }

        protected void Gather() {
            Agent.speed = RegularSpeed;
            SetAnimation(_attackAnimation, false);
                
            if (!Agent.pathPending && Agent.remainingDistance <= 0.1f) {
                                        
                IdleTimePassed += Time.deltaTime;

                if (_gatherPointsVisited > 3) {
                        
                    WasFolowingPlayer = false;
                    IdleTimePassed = 0;
                    _gatherPointsVisited = 0;
                        
                    return;
                }
            
                if (IdleTimePassed > 0.8f) {
                    _gatherPointsVisited++;
                    Agent.destination = GetGatherPoint();
                    IdleTimePassed = 0f;
                }
            }
            
        }

        protected Vector3 GetGatherPoint() {
            var coord = Utils.TransformWorldToLocalCoordinate(transform.position.x, transform.position.z);            
            while (true) {
                var positive = Random.Range(-1, 1) <= 0;
                int x;
                int y;
                if (positive) {
                    x = coord.X + Random.Range(1, 4);
                    y = coord.Y + Random.Range(1, 3);
                } else {
                    x = coord.X + Random.Range(-4, 0);
                    y = coord.X + Random.Range(-3, 0);
                }
                try {
                      if (App.AppManager.Instance.MazeInstance.Maze[x, y].Type == Tile.Variant.Empty) {
                          return Utils.TransformToWorldCoordinate(App.AppManager.Instance.MazeInstance.Maze[x, y].Position);
                      }                      
                }
                catch (IndexOutOfRangeException) {}
            }                        
        }

        protected void Attack() {
            Agent.isStopped = true;
            AttackTimePassed += Time.deltaTime;

            if (AttackTimePassed > TimeForAttack - 0.4f) {
                SetAnimation(_attackAnimation, true);
            }

            if (AttackTimePassed > TimeForAttack) {                
                Fire(TargetPosition);

                AttackTimePassed = 0f;
            }
            
        }

        protected bool CanAttack(Vector3 direction) {
            if (!Agent.pathPending && Agent.remainingDistance <= RangeOfAttack) {
                var pos = transform.position;
                var dir = direction;
                pos.y += 2.5f;
                dir.y += 1.5f;
                return !Physics.Linecast(pos, dir, out RaycastHit, _obstacleMask);
            }
            return false;
        }

        protected virtual void Fire(Vector3 direction) {            
            var pos = transform.position;
            pos.y += 1.5f;            
            direction.y += 2f;
            
            direction.z += Random.Range(-2, 3);
            direction.x += Random.Range(-2, 3);
            
            var activeItem = Instantiate(Weapon, pos, Quaternion.identity);
            var weapon = activeItem.GetComponent<Weapon>();
            
            activeItem.transform.LookAt(direction);
            weapon.Fire();
            NetworkServer.Spawn(activeItem);
            Destroy(weapon, 10.0f);            
        }
    }
}
