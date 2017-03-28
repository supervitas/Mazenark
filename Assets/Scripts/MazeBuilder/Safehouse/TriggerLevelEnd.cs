using App.EventSystem;
using UnityEngine;

namespace MazeBuilder.Safehouse {
    public class TriggerLevelEnd: MonoBehaviour {

        // Use this for initialization
        void Awake () {
		
        }
        void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Player")) {
                App.AppManager.Instance.EventHub.CreateEvent("maze:levelCompleted", new EventArguments(""));
            }
        }

    }
}
