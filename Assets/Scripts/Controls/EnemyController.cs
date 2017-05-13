﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lobby;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace Controls {
    [NetworkSettings(channel = 1, sendInterval = 0.5f)]
    public class EnemyController : NetworkBehaviour {
        [SerializeField]
        private Animator animator;
        private NavMeshAgent _agent;

        [SerializeField]
        private float enemyAgroRange = 40f;
        [SerializeField]
        private float enemyAngleVisibility = 30f;

        public readonly List <Vector3> Points = new List<Vector3>();
        private int _destPoint = 0;

        private List<Transform> _playersTransform;

        [HideInInspector]
        public bool CanPatrool = false;

        private bool _isAlive = true;


        private bool _hasTarget = false;



        // Server Only
        private void Start() {
            if (!isServer) return;
            _agent = GetComponent<NavMeshAgent>();
            _playersTransform = FindObjectOfType<LobbyGameManager>().PlayersTransforms;
            InvokeRepeating("CheckPlayersNear", 0, 0.5f);
            InvokeRepeating("UpdateEnemies", 0, 0.3f);
        }

        private void SetAnimation(string animationState, bool value) {
            if (animator.GetBool(animationState) != value) {
                animator.SetBool(animationState, value);
            }
        }

        public void SetIdleBehaivor() {
            SetAnimation("Idle", true);
        }

        public void GotoNextPoint() {
            // Set the agent to go to the currently selected destination.
            _agent.destination = Points[_destPoint];

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            _destPoint = (_destPoint + 1) % Points.Count;
        }

        public void Die() {
            if(!isServer) return;

            _isAlive = false;
            CancelInvoke("CheckPlayersNear");

            animator.SetBool("isDead", true);
            _agent.enabled = false;
        }

        private bool CheckPlayersNear() {
            foreach (var target in _playersTransform) {
                if (target == null) continue;

                var distance = Vector3.Distance(transform.position, target.position);
                var direction = _agent.destination - transform.position;

                if (direction == Vector3.zero) {
                    direction = transform.forward;
                }

                var angle = Vector3.Angle(direction, transform.forward);

                if (distance <= enemyAgroRange && angle < enemyAngleVisibility) {

                    _agent.autoBraking = true;

                    _agent.destination = target.position;
                    _agent.stoppingDistance = 2.5f;

                    _hasTarget = true;

                    return true;
                }
            }
            _hasTarget = false;
            _agent.autoBraking = false;

            return false;
        }

        private void GoToNextPointIfPossible() {
            if (!_agent.pathPending && _agent.remainingDistance <= 0.5f) {
                GotoNextPoint();
            }
        }

        private void UpdateEnemies() {
            if(!isServer && !_isAlive) return;

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

                if (_agent.remainingDistance > 2.5f) {
                    SetAnimation("Attack", false);
                }
                if (_agent.remainingDistance <= 2.5f) {
                    SetAnimation("Attack", true);
//                    Fire(transform.forward); // todo colider to enemy. No raycast
                }
            }

            if (CanPatrool && !_hasTarget) {
                GoToNextPointIfPossible();
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
