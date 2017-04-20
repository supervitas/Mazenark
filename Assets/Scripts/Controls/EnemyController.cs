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

        void Awake () {
            _agent = GetComponent<NavMeshAgent>();
            _agent.autoBraking = false;
        }

        void Start() {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
                 _playersTransform.Add(player.transform);
            }
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

        public void Die() {
            _isAlive = false;
            animator.SetBool("isDead", true);
            _agent.enabled = false;
        }

        private void Update () {
            if (!_isAlive) return;

            foreach (var target in _playersTransform) {
                if(target == null) return;

                var distance = Vector3.Distance(transform.position, target.position);
                if (_isAlive && distance <= enemyAgroRange) {
                    _agent.destination = target.position;
                    return;
                }
            }

            if (CanPatrool && !_agent.pathPending && _agent.remainingDistance <= 0.5f) {

                animator.SetBool("Moving", true);

                GotoNextPoint();
            }
        }
    }
}
