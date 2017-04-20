using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace Controls {
    [NetworkSettings(channel = 1, sendInterval = 0.3f)]
    public class EnemyController : NetworkBehaviour {
        [SerializeField]
        private Animator animator;
        private NavMeshAgent _agent;

        [SerializeField]
        private float enemyAgroRange = 15f;
        public readonly List <Vector3> Points = new List<Vector3>();
        private int _destPoint = 0;

        private readonly List<Transform> _playersTransform = new List<Transform>();

        [HideInInspector]
        public bool CanPatrool;

        private bool _isAlive = true;
        private bool _hasTarget;

        private void Awake () {
            _agent = GetComponent<NavMeshAgent>();
            _agent.autoBraking = false;
        }

        private void Start() {
            if(!isServer) return;

            foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
                 _playersTransform.Add(player.transform);
            }

            InvokeRepeating("CheckPlayersNear", 0, 1);
        }

        public void SetIdleBehaivor() {
            animator.SetBool("Idle", true);
            _agent.autoBraking = true;
        }

        public void GotoNextPoint() {
            // Set the agent to go to the currently selected destination.
            _agent.destination = Points[_destPoint];

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            _destPoint = (_destPoint + 1) % Points.Count;
        }

        public void Die() { //called from fireball onColide method
            CancelInvoke("CheckPlayersNear");

            _isAlive = false;
            animator.SetBool("isDead", true);
            _agent.enabled = false;
        }

        private bool CheckPlayersNear() {
            foreach (var target in _playersTransform) {
                if (target == null) continue;

                var distance = Vector3.Distance(transform.position, target.position);

                if (distance <= enemyAgroRange) {

                    _agent.autoBraking = true;

                    _agent.destination = target.position;
//                    _agent.stoppingDistance = 2f;

                    _hasTarget = true;

                    return true;
                }
            }
            animator.SetBool("Attack", false);
            animator.SetBool("Idle", true);
            _hasTarget = false;
            return false;
        }

        private void GoToNextPointIfPossible() {
            if (!_agent.pathPending && _agent.remainingDistance <= 0.5f) {

                animator.SetBool("Moving", true);

                GotoNextPoint();
            }
        }

        private void Update () {
            if (!_isAlive) return;


            if (_hasTarget) {
                if (_agent.remainingDistance > 2.5f) {
                    animator.SetBool("Moving", true);
                    animator.SetBool("Attack", false);
                }
                if (_agent.remainingDistance <= 2f) {
                    animator.SetBool("Attack", true);
                    Fire(transform.forward);
                    return;
                }
            }

            if (CanPatrool) {
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
                    go.SendMessage("TakeDamage", 100.0F, SendMessageOptions.DontRequireReceiver); // execute function on colided object.
                }
            }

        }
    }
}
