using App.EventSystem;
using UnityEngine;

namespace MazeBuilder.Safehouse {
    public class InDoorColisionNotifier : MonoBehaviour {
        void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Player")) {
                App.AppManager.Instance.EventHub.CreateEvent("safehouse:player_reached",
                    new EventArguments(other.gameObject.name));
            }
        }
    }

}