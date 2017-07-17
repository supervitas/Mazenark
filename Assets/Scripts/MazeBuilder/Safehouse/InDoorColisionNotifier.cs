using UnityEngine;

namespace MazeBuilder.Safehouse {
    public class InDoorColisionNotifier : MonoBehaviour {
        void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Player")) {
                GetComponentInParent<TriggerLevelEnd>().OnPlayerReached(other.gameObject.name);
            }
        }
    }
}