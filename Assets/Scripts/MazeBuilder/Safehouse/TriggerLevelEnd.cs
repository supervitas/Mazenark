using App;
using App.Eventhub;
using UnityEngine.Networking;

namespace MazeBuilder.Safehouse {
    public class TriggerLevelEnd: NetworkBehaviour {
        public void OnPlayerReached(string playerName) {
            if (!isServer) return;
            AppManager.Instance.EventHub.CreateEvent("maze:levelCompleted", new EventArguments(playerName));
            NetworkEventHub.Instance.RpcPublishEvent("maze:levelCompleted", playerName);           
        }
    }
}
