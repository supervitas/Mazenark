using App.EventSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace MazeBuilder.Safehouse {
    public class TriggerLevelEnd: NetworkBehaviour {

        // Use this for initialization
        void Awake () {
		    App.AppManager.Instance.EventHub.Subscribe("safehouse:player_reached", OnPlayerReached, this);
        }

        void OnPlayerReached(object sender, EventArguments eventArguments) {
            App.NetworkEventHub.Instance.RpcPublishEvent("maze:levelCompleted", eventArguments.Message);
        }

    }
}
