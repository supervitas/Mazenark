using App.EventSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace MazeBuilder.Safehouse {
    public class TriggerLevelEnd: NetworkBehaviour {

        public void OnPlayerReached(string playerName) {
            App.NetworkEventHub.Instance.RpcPublishEvent("maze:levelCompleted", playerName);
        }

    }
}
