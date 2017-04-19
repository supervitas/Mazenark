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

        private List<Transform> _playersTransform = new List<Transform>();

        [HideInInspector]
        public bool canPatrool;


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

            ToggleNetworkComponents(false);
        }

        public void GotoNextPoint() {
            // Set the agent to go to the currently selected destination.
            _agent.destination = Points[_destPoint];

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            _destPoint = (_destPoint + 1) % Points.Count;
        }

        public void Die() {
            animator.SetBool("isDead", true);
            canPatrool = false;
            _agent.enabled = false;
        }

        private void Update () {
            foreach (var target in _playersTransform) {
                var distance = Vector3.Distance(transform.position, target.position);
                if (canPatrool && distance <= enemyAgroRange) {
                    _agent.destination = target.position;
                    ToggleNetworkComponents(true);
                    return;
                }
            }

            if (canPatrool && !_agent.pathPending && _agent.remainingDistance <= 0.5f) {
                animator.SetBool("Moving", true);
                GotoNextPoint();
            }
        }

        private void ToggleNetworkComponents(bool status) {
            GetComponent<NetworkAnimator>().enabled = status;
            GetComponent<NetworkTransform>().enabled = status;
        }
    }
}
