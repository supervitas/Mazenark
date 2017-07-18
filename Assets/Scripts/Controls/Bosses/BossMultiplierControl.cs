using MazeBuilder.Utility;
using UnityEngine;


namespace Controls.Bosses {    
    public class BossMultiplierControl : BasicBossControl {
        
        protected override void Update() {
            if (!IsAlive || !isServer) return;

            if (Agent.velocity != Vector3.zero) {
                SetAnimation("Moving", true);
                SetAnimation("Idle", false);
            }
            else {
                SetAnimation("Idle", true);
            }

            if (CheckPlayersNear(out TargetPosition)) {
                var direction = Agent.destination - transform.position;

                if (direction != Vector3.zero) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                }

                Agent.autoBraking = true;

                Agent.destination = TargetPosition;
                Agent.stoppingDistance = 5f;

                if (Agent.remainingDistance > 5f) {
                    SetAnimation("Attack", false);
                    AttackTimePassed = 0;
                }
                else if (!Agent.pathPending && Agent.remainingDistance <= 5f) {
                    SetAnimation("Attack", true);
                    Fire(TargetPosition);
                }               
            }
            Agent.destination = Utils.GetDefaultPositionVector(SpawnRoom.Center);
        }     
    }
}

