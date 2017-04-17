using App.EventSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace MazeBuilder.Safehouse {
    public class TriggerLevelEnd: NetworkBehaviour {

        // Use this for initialization
        void Awake () {
		
        }
        void OnTriggerEnter(Collider other) {
            Debug.Log(isServer);
            Debug.Log(other.gameObject.name);

            if (isServer) {
                if (other.gameObject.CompareTag("Player")) {
                    App.NetworkEventHub.Instance.RpcPublishEvent("maze:levelCompleted", other.gameObject.name);
                }
            }
        }

    }
}
