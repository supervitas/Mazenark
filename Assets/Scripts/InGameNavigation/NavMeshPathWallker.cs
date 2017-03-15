using UnityEngine;
using UnityEngine.AI;

namespace InGameNavigation {
    public class NavMeshPathWallker : MonoBehaviour {

        public Transform goal;

        void Start () {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = goal.position;
        }

        private void Update() {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = goal.position;

        }
    }
}
