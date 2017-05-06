using UnityEngine;
using UnityEngine.Networking;

namespace MazeBuilder.Safehouse {
    public class TriggerLevelEnd: NetworkBehaviour {
        public void OnPlayerReached(string playerName) {
            if (!isServer) return;
            App.NetworkEventHub.Instance.RpcPublishEvent("maze:levelCompleted", playerName);
            Destroy(GameObject.Find(playerName));
        }
    }
}
