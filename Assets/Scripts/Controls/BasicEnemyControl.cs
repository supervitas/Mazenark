using System;
using System.Collections;
using System.Collections.Generic;
using CharacterControllers;
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
        
        [ClientRpc]
        protected void RpcStartDisolve() {
            GetComponent<Disolve>().BeginDisolve();                                    
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
            
            Invoke(nameof(RpcStartDisolve), 1.1f);       
        }  

        protected virtual bool CheckPlayersNear(out Vector3 playerTarget) {                        
            foreach (var target in _playersTransform) {
                if (target == null) continue;

                var distance = Vector3.Distance(transform.position, target.position);
                
                var pos = transform.position;
                var dir = target.position;
                pos.y += 2.5f;
                dir.y += 1.5f;
                                
                Debug.DrawLine(pos, dir);
                
                if (distance <= EnemyAgroRange && !Physics.Linecast(pos, dir, out RaycastHit, _obstacleMask)) {                    
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
                Agent.speed = RegularSpeed;
                
                if (_gatherPointsVisited == 0) {
                    _gatherPointsVisited++;                   
//                    GoToNextGatherPoint();
//                    return;
                }
                if (!Agent.pathPending && Agent.remainingDistance <= 0.5f) {
                    
                    IdleTimePassed += Time.deltaTime;

                    if (IdleTimePassed > 0.5f) {
//                        transform.rotation = Quaternion.Slerp(transform.rotation,
//                            Quaternion.LookRotation(transform.position - Agent.destination), 0.1f);
                    }
                    
                    if (IdleTimePassed > 1.0f) {
//                        transform.rotation = Quaternion.Slerp(transform.rotation,
//                            Quaternion.LookRotation(2 * transform.position - lookingAt.position), 0.1f);
                    }
                    
                    if (IdleTimePassed > 2.5f) {
                        WasFolowingPlayer = false;
                        _gatherPointsVisited = 0;
                        IdleTimePassed = 0;
                    }
                }


                return;
            }
            
            Agent.speed = RegularSpeed;
            
            SetAnimation(_attackAnimation, false);
                    
            if (CanPatrool && !Agent.pathPending && Agent.remainingDistance <= 0.5f || WasFolowingPlayer) { // Continue patrool
                WasFolowingPlayer = false;
                
                Agent.destination = Points[_destPoint];
                _destPoint = (_destPoint + 1) % Points.Count;
            }
        }

        protected void GoToNextGatherPoint() {
            var coord = Utils.TransformWorldToLocalCoordinate(transform.position.x, transform.position.z);
            var position = GetRandomEmptyCoordinate(coord);
            Agent.destination = position;
        }
        protected Vector3 GetRandomEmptyCoordinate(Coordinate coordinate) {
            var x = coordinate.X + Random.Range(-2, 2);
            var y = coordinate.Y + Random.Range(-2, 2);
            try {
                return App.AppManager.Instance.MazeInstance.Maze[x, y].Type == Tile.Variant.Empty ?
                    Utils.TransformToWorldCoordinate(App.AppManager.Instance.MazeInstance.Maze[x, y].Position) :
                    GetRandomEmptyCoordinate(coordinate);
            }
            catch (IndexOutOfRangeException) {
                return GetRandomEmptyCoordinate(coordinate);
            }
        }

        protected void Attack() {
            Agent.isStopped = true;
            AttackTimePassed += Time.deltaTime;

            if (AttackTimePassed > TimeForAttack) {

                SetAnimation(_attackAnimation, true);
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
            
            direction.z += Random.Range(-2, 3); // Разброс
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
