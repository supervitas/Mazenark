using System.Collections.Generic;
using CharacterControllers;
using Lobby;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace Controls.Enemies {    
    public abstract class BasicEnemyControl : NetworkBehaviour {
        [SerializeField]
        protected Animator _animator;
        protected NavMeshAgent _agent;

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
        
        protected void Start() {            
            if (!isServer) return;
            _agent = GetComponent<NavMeshAgent>();  
            _playersTransform = FindObjectOfType<LobbyGameManager>().PlayersTransforms;
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
            Points.Add(patroolPoint);                        
        }

        public void SetPatrool(bool patrool) {
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

        protected bool CheckPlayersNear(out Vector3 playerTarget) {                        
            foreach (var target in _playersTransform) {
                if (target == null) continue;

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

        protected void Update() {
            if(!_isAlive) return;
            
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
                
                _agent.autoBraking = true;
                
                _agent.destination = targetPosition;
                _agent.stoppingDistance = 5f;                                                

                if (_agent.remainingDistance > 5f) {
                    SetAnimation("Attack", false);
                    _attackTimePassed = 0;
                } else if (!_agent.pathPending && _agent.remainingDistance <= 5f) {                    
                    SetAnimation("Attack", true);                    
                    Fire(targetPosition);                    
                }
                return;
            }
            
            if (_canPatrool && !_agent.pathPending && _agent.remainingDistance <= 0.5f) {
                GotoNextPoint();
            }
        }

        protected virtual void Fire(Vector3 direction) {
            RaycastHit hit;
            var pos = transform.position;
            pos.y = 1.5f;
            direction.y = 1.5f;
            Debug.DrawLine(pos, direction, Color.red);
            if (Physics.Raycast(pos, direction, out hit, 6f)) {               
                var go = hit.transform.gameObject;
                Debug.Log(go.tag);
                if (go.CompareTag("Player")) {
//                    go.GetComponent<ServerCharacterController>().TakeDamage(100);                    
                }
            }
        }
    }
}
