using UnityEngine;
using UnityEngine.AI;

namespace InGameNavigation {
    public class NavMeshPathWallker : MonoBehaviour {
		

        public Transform goal;
		public float distanceWhereToStop = 3.0f;

		void Start () {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = goal.position;
        }

        private void Update() {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();

			if ((agent.transform.position - goal.position).magnitude < distanceWhereToStop) {
				agent.destination = agent.transform.position;
				return;
			}

			agent.destination = goal.position;

		}
    }
}
