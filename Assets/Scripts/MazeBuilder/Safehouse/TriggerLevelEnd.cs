using App;
using App.Eventhub;
using GameSystems;
using UnityEngine.Networking;

namespace MazeBuilder.Safehouse {
    public class TriggerLevelEnd: NetworkBehaviour {
        public void OnPlayerReached(string playerName) {
            if (!isServer) return;
            FindObjectOfType<GameManager>().PlayerCompletedLevel(playerName);
            AppManager.Instance.EventHub.CreateEvent("maze:levelCompleted", new EventArguments(playerName));
            NetworkEventHub.Instance.RpcPublishEvent("maze:levelCompleted", playerName);           
        }
    }
}
