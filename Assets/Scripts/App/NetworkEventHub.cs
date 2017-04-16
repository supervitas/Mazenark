using App.EventSystem;
using UnityEngine.Networking;

namespace App {
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