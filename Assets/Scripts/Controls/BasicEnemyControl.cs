using System.Collections.Generic;
using CharacterControllers;
using GameSystems;
using Loot;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using Weapons;

namespace Controls {    
    public abstract class BasicEnemyControl : NetworkBehaviour {
        [SerializeField]
        protected Animator _animator;
        protected NavMeshAgent _agent;
        
        [SerializeField]
        protected GameObject _weapon;

        [SerializeField]
        protected float enemyAgroRange = 20f;
        [SerializeField]
        private float enemyAngleVisibility = 30f;

        protected readonly List <Vector3> Points = new List<Vector3>();
        protected int _destPoint = 0;

        protected List<Transform> _playersTransform; 

        protected bool _isAlive = true;

        protected bool _hasTarget = false;

        protected bool _canPatrool = false;

        [SerializeField]
        protected float TimeForAttack = 0.5f;
        
        protected float _attackTimePassed = 0f;

        protected Vector3 targetPosition;

        public override void OnStartServer() {                    
            if (!isServer) return;
            _agent = GetComponent<NavMeshAgent>();  
            _playersTransform = FindObjectOfType<GameManager>().GetPlayersTransforms();
        }

        public bool IsAlive() {
            return _isAlive;
        }

        protected void SetAnimation(string animationState, bool value) {
            if (_animator.GetBool(animationState) != value) {
                _animator.SetBool(animationState, value);
            }
        }        

        public void AddPatroolPoint(Vector3 patroolPoint) {
            if (!isServer) return;
            Points.Add(patroolPoint);                        
        }

        public void SetPatrool(bool patrool) {
            if (!isServer) return;
            _canPatrool = patrool;
        }

        public void GotoNextPoint() {             
            _agent.destination = Points[_destPoint];
            _destPoint = (_destPoint + 1) % Points.Count;
        }

        public void Die() {
            if(!isServer) return;

            _isAlive = false;                         
            
            SetAnimation("isDead", true);
            _agent.isStopped = true;
        }

        protected virtual bool CheckPlayersNear(out Vector3 playerTarget) {                        
            foreach (var target in _playersTransform) {
                if (target == null ) continue;

                var distance = Vector3.Distance(transform.position, target.position);
                var direction = _agent.destination - transform.position;
                
                if (direction == Vector3.zero) {
                    direction = transform.forward;
                }

                var angle = Vector3.Angle(direction, transform.forward);
                //&& angle < enemyAngleVisibility
                if (distance <= enemyAgroRange) {
                    playerTarget = target.position;
                    return true;
                }
            }
            playerTarget = Vector3.zero;           
            return false;
        }

        protected virtual void Update() {            
            if(!_isAlive || !isServer) return;
            
            if (_agent.velocity != Vector3.zero) {
                SetAnimation("Moving", true);
                SetAnimation("Idle", false);
            } else {         
                SetAnimation("Idle", true);
            }                        
                       
            if (CheckPlayersNear(out targetPosition)) {
                var direction = _agent.destination - transform.position;

                if (direction != Vector3.zero) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                }
                
                var angle = Vector3.Angle(direction, transform.forward);
                
                
                _agent.autoBraking = true;
                
                _agent.destination = targetPosition;
                _agent.stoppingDistance = 15f;                                                
               
                if (_agent.remainingDistance > 15f) {
                    
                    _attackTimePassed = 0;
                    
                    SetAnimation("Attack", false);
                    
                } 
                
                if (!_agent.pathPending && _agent.remainingDistance <= 15f && angle <= 60f) {
                    
                    _attackTimePassed += Time.deltaTime;

                    if (_attackTimePassed > TimeForAttack) {

                        SetAnimation("Attack", true);
                        Fire(targetPosition);

                        _attackTimePassed = 0f;
                        
                    }
                }                 
                return;
            }
            
            SetAnimation("Attack", false);
            
            if (_canPatrool && !_agent.pathPending && _agent.remainingDistance <= 0.5f) {
                GotoNextPoint();
            }
        }                

        protected virtual void Fire(Vector3 direction) {                              
            var pos = transform.position;
            pos.y += 1.5f;
            direction.y += 2f;
            var activeItem = Instantiate(_weapon, pos, Quaternion.identity);
            var weapon = activeItem.GetComponent<Weapon>();            
            activeItem.transform.LookAt(direction);
            weapon.Fire();
            NetworkServer.Spawn(activeItem);
            Destroy(weapon, 10.0f);            
        }
    }
}
