using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace Controls {
    public class EnemyController : MonoBehaviour {
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
            foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
                 _playersTransform.Add(player.transform);
            }

            InvokeRepeating("CheckPlayersNear", 0, 2);
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
                    _agent.stoppingDistance = 1.5f;

                    _hasTarget = true;

                    return true;
                }
            }
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
                if (_agent.remainingDistance <= 1.5f) {
                    animator.SetBool("Attack", true);
                    return;
                }
                if (_agent.remainingDistance > enemyAgroRange + 5f) {
                    animator.SetBool("Attack", false);
                    animator.SetBool("Idle", true);

                    _hasTarget = false;
                    GotoNextPoint();
                    return;
                }
            }

            if (CanPatrool) {
                GoToNextPointIfPossible();
            }

        }
    }
}
