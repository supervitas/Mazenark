using App.Eventhub;
using UnityEngine.Networking;


namespace App {
    [NetworkSettings(channel = 0, sendInterval = 0.5f)]
    public class NetworkEventHub : NetworkBehaviour {
        public static NetworkEventHub Instance { get; private set; }

        //Singletone which starts firstly then other scripts;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }

        [ClientRpc]
        public void RpcPublishEvent(string eventName, string args) {
            AppManager.Instance.EventHub.CreateEvent(eventName, new EventArguments(args));
        }
    }
}