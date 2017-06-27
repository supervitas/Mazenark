using CharacterControllers.Enemies.Bosses;
using MazeBuilder.Utility;
using UnityEngine;


namespace Controls.Bosses {    
    public class BossShieldedControl : BasicBossControl {

		public ServerBossShieldedController Controller { get; set; }
	
        protected override void Update() {
            if (!_isAlive) return;

            if (_agent.velocity != Vector3.zero) {
                SetAnimation("Moving", true);
                SetAnimation("Idle", false);
            }
            else {
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
                }
                else if (!_agent.pathPending && _agent.remainingDistance <= 5f) {
                    SetAnimation("Attack", true);
                    Fire(targetPosition);
                }               
            }
            _agent.destination = Utils.GetDefaultPositionVector(SpawnRoom.Center);
        }


        protected override void Fire(Vector3 direction) {
            RaycastHit hit;
            var pos = transform.position;

            pos.y = 1f;

            if (Physics.Raycast(pos, direction, out hit, 2.5f)) {
                var go = hit.transform.gameObject;
                if (go.CompareTag("Player")) {
                    go.SendMessage("TakeDamage", 50.0F,
                        SendMessageOptions.DontRequireReceiver); // execute function on colided object.
                }
            }
        }      
    }
}
