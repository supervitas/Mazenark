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

                if (Agent.remainingDistance > RangeOfAttack) {
                    AttackTimePassed = 0;
                    SetAnimation(_attackAnimation, false);
                }
                if (!Agent.pathPending && Agent.remainingDistance <= RangeOfAttack ) {
                    AttackTimePassed += Time.deltaTime;

                    if (AttackTimePassed > TimeForAttack) {

                        SetAnimation(_attackAnimation, true);
                        Fire(TargetPosition);

                        AttackTimePassed = 0f;
                    }
                }
            }
            Agent.destination = Utils.GetDefaultPositionVector(SpawnRoom.Center);
        }     
    }
}

