using MazeBuilder.Utility;
using UnityEngine;


namespace Controls.Bosses {    
    public class BossMultiplierControl : BasicBossControl {
        
        protected override void Update() {
            if (!IsAlive || !isServer) return;

            SetAnimation(Agent.velocity != _zeroVector ? _movingAnimation : _idleAnimation, true);


            if (CheckPlayersNear(out TargetPosition)) {
                var direction = Agent.destination - transform.position;

                if (direction != Vector3.zero) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                }

                Agent.autoBraking = true;

                Agent.destination = TargetPosition;
                Agent.stoppingDistance = 5f;

                if (Agent.remainingDistance > 5f) {
                    SetAnimation(_attackAnimation, false);
                    AttackTimePassed = 0;
                }
                else if (!Agent.pathPending && Agent.remainingDistance <= 5f) {
                    SetAnimation(_attackAnimation, true);
                    Fire(TargetPosition);
                }               
            }
            Agent.destination = Utils.GetDefaultPositionVector(SpawnRoom.Center);
        }     
    }
}

