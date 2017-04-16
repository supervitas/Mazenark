using System.Collections.Generic;
using System.Linq;
using MazeBuilder;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using MazeBuilder.Utility;

namespace Controls {
    public class EnemyController : MonoBehaviour {
        [SerializeField]
        private Animator animator;
        private NavMeshAgent _agent;

        public readonly List <Vector3> Points = new List<Vector3>();
        private int _destPoint = 0;

        [HideInInspector]
        public bool isPatrool;


        void Awake () {
            animator.SetBool("Idle", true);
            _agent = GetComponent<NavMeshAgent>();
            _agent.autoBraking = false;
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

        // Update is called once per frame
        private void LateUpdate () {
            if (_agent.velocity != Vector3.zero) {
                animator.SetBool("Moving", true);
                animator.SetBool("Idle", false);
            }
            if (isPatrool && !_agent.pathPending && _agent.remainingDistance <= 0.5f) {
                GotoNextPoint();
            }
        }
    }
}
