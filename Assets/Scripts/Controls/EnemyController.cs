using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Controls {
    public class EnemyController : MonoBehaviour {
        [SerializeField]
        private Animator animator;
        private NavMeshAgent _agent;

        [SerializeField]
        private float enemyAgroRange = 10f;
        public readonly List <Vector3> Points = new List<Vector3>();
        private int _destPoint = 0;

        private List<Transform> _playersTransform = new List<Transform>();

        [HideInInspector]
        public bool isPatrool;


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
            animator.SetBool("isDead", true);
            isPatrool = false;
            _agent.autoBraking = true;
            _agent.velocity = Vector3.zero;
            _agent.ResetPath();
        }


        // Update is called once per frame
        private void Update () {
//            foreach (var target in _playersTransform) {
//                var distance = Vector3.Distance(transform.position, target.position);
//                if (distance <= enemyAgroRange) {
//                    _agent.destination = target.position;
//                    break;
//                }
//
//            }

            if (isPatrool && !_agent.pathPending && _agent.remainingDistance <= 0.5f) {
                animator.SetBool("Moving", true);
                GotoNextPoint();
            }
        }
    }
}
