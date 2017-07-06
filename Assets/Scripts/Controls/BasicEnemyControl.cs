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
        protected Animator Animator;
        protected NavMeshAgent Agent;
        
        [SerializeField]
        protected GameObject Weapon;

        [SerializeField]
        protected float EnemyAgroRange = 20f;
        [SerializeField]
        protected float EnemyAngleVisibility = 30f;

        protected readonly List <Vector3> Points = new List<Vector3>();
        private int _destPoint = 0;

        protected List<Transform> _playersTransform;

        public bool IsAlive { get; private set; }        

        protected bool CanPatrool = false;
        

        protected float RegularSpeed;

        [SerializeField]
        protected float TimeForAttack = 0.5f;
        
        protected float AttackTimePassed = 0f;
        
        [SerializeField]
        protected float SpeedBoostOnAgro = 0.7f;
        
        [SerializeField]
        [Range(3, 40)]
        protected float RangeOfAttack = 5f;
        
        
        protected Vector3 TargetPosition;
        protected bool WasFolowingPlayer = false;
        
        
        
        public override void OnStartServer() {                    
            if (!isServer) return;
            
            Agent = GetComponent<NavMeshAgent>();
            
            _playersTransform = FindObjectOfType<GameManager>().GetPlayersTransforms();
            
            RegularSpeed = Agent.speed;
            
            IsAlive = true;
        }

        protected void SetAnimation(string animationState, bool value) {
            if (Animator.GetBool(animationState) != value) {
                Animator.SetBool(animationState, value);
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

        public void Die() {
            if(!isServer) return;

            IsAlive = false;                         
            
            SetAnimation("isDead", true);
            Agent.isStopped = true;
        }

        protected virtual bool CheckPlayersNear(out Vector3 playerTarget) {                        
            foreach (var target in _playersTransform) {
                if (target == null ) continue;

                var distance = Vector3.Distance(transform.position, target.position);
                var direction = Agent.destination - transform.position;
                
                if (direction == Vector3.zero) {
                    direction = transform.forward;
                }

                var angle = Vector3.Angle(direction, transform.forward);
                
                if (distance <= EnemyAgroRange && angle < EnemyAngleVisibility) {
                    playerTarget = target.position;
                    return true;
                }
            }
            playerTarget = Vector3.zero;           
            return false;
        }

        protected virtual void Update() {            
            if (!IsAlive || !isServer) return;

            SetAnimation(Agent.velocity != Vector3.zero ? "Moving" : "Idle", true);

            if (CheckPlayersNear(out TargetPosition)) {

                WasFolowingPlayer = true;
                
                var direction = Agent.destination - transform.position;

                Agent.speed = RegularSpeed * SpeedBoostOnAgro;

                if (direction != Vector3.zero) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                }
                
                Agent.autoBraking = true;
                
                Agent.destination = TargetPosition;
                Agent.stoppingDistance = RangeOfAttack;
                                

                if (Agent.remainingDistance > RangeOfAttack) {

                    AttackTimePassed = 0;

                    SetAnimation("Attack", false);

                }

                if (!Agent.pathPending && Agent.remainingDistance <= RangeOfAttack) {

                    AttackTimePassed += Time.deltaTime;

                    if (AttackTimePassed > TimeForAttack) {

                        SetAnimation("Attack", true);
                        Fire(TargetPosition);

                        AttackTimePassed = 0f;

                    }
                }
                return;
            }
     
                        
            Agent.speed = RegularSpeed;
            
            SetAnimation("Attack", false);
        
            
            if (CanPatrool && !Agent.pathPending && Agent.remainingDistance <= 0.5f || WasFolowingPlayer) {
                WasFolowingPlayer = false;
                GotoNextPoint();

            }
        }                

        protected virtual void Fire(Vector3 direction) {
//            Debug.DrawLine(transform.position, direction);
//            RaycastHit hit;
//            var pos = transform.position;
//            pos.y += 2.5f;
//            direction.y += 2f;

//            if (Physics.Raycast(transform.position, direction, out hit)) {
                
//                Debug.Log(hit.transform.gameObject.name);
                
//            }
            var pos = transform.position;
            pos.y += 1.5f;
            direction.y += 2f;
            var activeItem = Instantiate(Weapon, pos, Quaternion.identity);
            var weapon = activeItem.GetComponent<Weapon>();            
            activeItem.transform.LookAt(direction);
            weapon.Fire();
            NetworkServer.Spawn(activeItem);
            Destroy(weapon, 10.0f);            
        }
    }
}
