using System.Collections.Generic;
using Lobby;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace Controls.Bosses {
    [NetworkSettings(channel = 1, sendInterval = 0.2f)]
    public abstract class BasicBossControl : NetworkBehaviour {
        [SerializeField]
        protected Animator Animator;
        protected NavMeshAgent Agent;
        protected Room SpawnRoom;
        
        [SerializeField]
        protected float EnemyAgroRange = 50f;                                  

        protected List<Transform> PlayersTransform;

        protected bool IsAlive = true;

        protected bool HasTarget = false;
        protected Vector3[] RoomBounds = new Vector3[4];
        
        protected void Start() {
            if (!isServer) return;           
            SetAnimation("Idle", true);
            Agent = GetComponent<NavMeshAgent>();
            PlayersTransform = FindObjectOfType<LobbyGameManager>().PlayersTransforms;            
            InvokeRepeating("CheckPlayersNear", 0, 0.3f);
            InvokeRepeating("UpdateBoss", 0, 0.2f);            
        }

        public void SetSpawnRoom(Room room) {              
            SpawnRoom = room;            
            RoomBounds[0] = Utils.TransformToWorldCoordinate(new Coordinate(SpawnRoom.BottomLeftCorner.X - 1, SpawnRoom.BottomLeftCorner.Y + 1));            
            RoomBounds[1] = Utils.TransformToWorldCoordinate(new Coordinate(SpawnRoom.TopLeftCorner.X - 1, SpawnRoom.TopLeftCorner.Y - 1));
            RoomBounds[2] = Utils.TransformToWorldCoordinate(new Coordinate(SpawnRoom.BottomRightCorner.X + 1, SpawnRoom.BottomRightCorner.Y + 1));
            RoomBounds[3] = Utils.TransformToWorldCoordinate(new Coordinate(SpawnRoom.TopRightCorner.X + 1, SpawnRoom.TopRightCorner.Y - 1));
        }
        
        public Room GetSpawnRoom() { return SpawnRoom; }


        protected void SetAnimation(string animationState, bool value) {
            if (Animator.GetBool(animationState) != value) {
                Animator.SetBool(animationState, value);
            }
        }                

        public void Die() {
            if(!isServer) return;

            IsAlive = false;
            CancelInvoke("CheckPlayersNear");

            SetAnimation("isDead", true);
            Agent.enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
        }

        protected bool TargetInRoom(Vector3 targetPosition) {                
            if (SpawnRoom == null) return false;
            if (targetPosition.z < RoomBounds[0].z && targetPosition.z > RoomBounds[1].z && 
                targetPosition.x > RoomBounds[0].x && targetPosition.x < RoomBounds[2].x ) {
                Debug.Log("intercets");
                return true;
            }           
            return false;
        }

        protected void CheckPlayersNear() {            
            foreach (var target in PlayersTransform) {                
                if (target == null) continue;

                var distance = Vector3.Distance(transform.position, target.position);
                var direction = Agent.destination - transform.position;

                if (direction == Vector3.zero) {
                    direction = transform.forward;

                }

                var angle = Vector3.Angle(direction, transform.forward);                
                if (distance <= EnemyAgroRange && TargetInRoom(target.position)) {

                    Agent.autoBraking = true;

                    Agent.destination = target.position;
                    Agent.stoppingDistance = 3f;

                    HasTarget = true;
                    return;
                }
            }
            SetAnimation("Attack", false);
            HasTarget = false;
            Agent.autoBraking = false;
        }


        protected abstract void UpdateBoss();

        private void Fire(Vector3 direction) {
            RaycastHit hit;
            var pos = transform.position;

            pos.y = 1f;

            if (Physics.Raycast(pos, direction, out hit, 2.5f)) {
                var go = hit.transform.gameObject;
                if (go.CompareTag("Player")) {
                    go.SendMessage("TakeDamage", 50.0F, SendMessageOptions.DontRequireReceiver); // execute function on colided object.
                }
            }

        }
    }
}
