using System.Collections.Generic;
using CharacterControllers;
using GameEnv.GameEffects;
using GameSystems;
using Items;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

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
        
        [SerializeField]
        protected float SpeedBoostOnAgro = 0.7f;
        
        [SerializeField]
        [Range(3, 40)]
        protected float RangeOfAttack = 6f;
        
        
        protected Vector3 TargetPosition;
        protected bool WasFolowingPlayer = false;
                        
        
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

        public void GotoNextPoint() {             
            Agent.destination = Points[_destPoint];
            _destPoint = (_destPoint + 1) % Points.Count;
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
            
            GetComponent<Disolve>().BeginDisolve(timeOfDeath);
        }

        protected virtual bool CheckPlayersNear(out Vector3 playerTarget) {                        
            foreach (var target in _playersTransform) {
                if (target == null) continue;

                var distance = Vector3.Distance(transform.position, target.position);
//                var direction = Agent.destination - transform.position;
//                
//                if (direction == Vector3.zero) {
//                    direction = transform.forward;
//                }
//
//                var angle = Vector3.Angle(direction, transform.forward);
                
                if (distance <= EnemyAgroRange) {                    
                    playerTarget = target.position;
                    return true;
                }
            }
            playerTarget = _zeroVector;           
            return false;
        }        

        protected virtual void Update() {            
            if (!IsAlive || !isServer) return;

            SetAnimation(Agent.velocity != _zeroVector ? _movingAnimation : _idleAnimation, true);

            if (CheckPlayersNear(out TargetPosition)) {

                WasFolowingPlayer = true;
                
                var direction = Agent.destination - transform.position;

                Agent.speed = RegularSpeed * SpeedBoostOnAgro;
                
                if (direction != _zeroVector) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                }
                
                Agent.autoBraking = true;
                
                Agent.destination = TargetPosition;
                Agent.isStopped = false;
                                

                if (Agent.remainingDistance > RangeOfAttack) {

                    AttackTimePassed = 0;

                    SetAnimation(_attackAnimation, false);

                }
                
                
                if (!Agent.pathPending && Agent.remainingDistance <= RangeOfAttack && CanAttack(TargetPosition)) {
                    
                    Agent.isStopped = true;
                    AttackTimePassed += Time.deltaTime;

                    if (AttackTimePassed > TimeForAttack) {

                        SetAnimation(_attackAnimation, true);
                        Fire(TargetPosition);

                        AttackTimePassed = 0f;

                    }
                }                
                return;
            }
                                         
            Agent.speed = RegularSpeed;
            
            SetAnimation(_attackAnimation, false);
        
            
            if (CanPatrool && !Agent.pathPending && Agent.remainingDistance <= 0.5f || WasFolowingPlayer) {
                WasFolowingPlayer = false;
                GotoNextPoint();
            }
        }

        protected bool CanAttack(Vector3 direction) {                        
            var pos = transform.position;
            var dir = direction;
            pos.y += 2.5f;           
            dir.y += 1.5f;           
            
            return !Physics.Linecast(pos, dir, out RaycastHit, _obstacleMask);
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
