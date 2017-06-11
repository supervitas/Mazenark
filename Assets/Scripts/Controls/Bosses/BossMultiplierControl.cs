using MazeBuilder.Utility;
using UnityEngine;


namespace Controls.Bosses {    
    public class BossMultiplierControl : BasicBossControl {
                        
        protected override void UpdateBoss() {            
            if(!IsAlive) return;            

            if (Agent.velocity != Vector3.zero) {
                SetAnimation("Moving", true);
                SetAnimation("Idle", false);
            }

            if (Agent.velocity == Vector3.zero) {
                SetAnimation("Idle", true);
            }

            if (HasTarget) {
                var direction = Agent.destination - transform.position;

                if (direction != Vector3.zero) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                }

                if (Agent.remainingDistance > 3f) {
                    SetAnimation("Attack", false);
                }
                if (Agent.remainingDistance <= 3f) {
                    SetAnimation("Attack", true);
//                    Fire(transform.forward); // todo colider to enemy. No raycast
                }
            }

        }

        
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
