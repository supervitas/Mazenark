using System.Collections.Generic;
using Lobby;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace Controls.Bosses {
    [NetworkSettings(channel = 1, sendInterval = 0.2f)]
    public class BossMultiplierControl : NetworkBehaviour {
        [SerializeField]
        private Animator _animator;
        private NavMeshAgent _agent;

        [SerializeField]
        private float enemyAgroRange = 50f;                                  

        private List<Transform> _playersTransform;

        private bool _isAlive = true;

        private bool _hasTarget = false;

        private Transform _spawnTransform;
        
        private void Start() {
            if (!isServer) return;
            SetAnimation("Idle", true);
            _agent = GetComponent<NavMeshAgent>();
            _playersTransform = FindObjectOfType<LobbyGameManager>().PlayersTransforms;
            InvokeRepeating("CheckPlayersNear", 0, 0.3f);
            InvokeRepeating("UpdateBoss", 0, 0.1f);
        }

        private void SetAnimation(string animationState, bool value) {
            if (_animator.GetBool(animationState) != value) {
                _animator.SetBool(animationState, value);
            }
        }    

        public void Die() {
            if(!isServer) return;

            _isAlive = false;
            CancelInvoke("CheckPlayersNear");

            SetAnimation("isDead", true);
            _agent.enabled = false;
        }

        private void CheckPlayersNear() {
            foreach (var target in _playersTransform) {
                if (target == null) continue;

                var distance = Vector3.Distance(transform.position, target.position);
                var direction = _agent.destination - transform.position;

                if (direction == Vector3.zero) {
                    direction = transform.forward;

                }

                var angle = Vector3.Angle(direction, transform.forward);                
                if (distance <= enemyAgroRange && angle < 30 /*&& Utils.TransformWorldToLocalCoordinate(target.position.x, target.position.z).*/) {

                    _agent.autoBraking = true;

                    _agent.destination = target.position;
                    _agent.stoppingDistance = 3f;

                    _hasTarget = true;
                    return;
                }
            }
            SetAnimation("Attack", false);
            _hasTarget = false;
            _agent.autoBraking = false;
        }

        private void UpdateBoss() {
            if(!_isAlive) return;

            if (_agent.velocity != Vector3.zero) {
                SetAnimation("Moving", true);
                SetAnimation("Idle", false);
            }

            if (_agent.velocity == Vector3.zero) {
                SetAnimation("Idle", true);
            }

            if (_hasTarget) {
                var direction = _agent.destination - transform.position;

                if (direction != Vector3.zero) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                }

                if (_agent.remainingDistance > 3f) {
                    SetAnimation("Attack", false);
                }
                if (_agent.remainingDistance <= 3f) {
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
