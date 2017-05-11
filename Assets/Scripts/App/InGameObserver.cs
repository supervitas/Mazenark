using App.Eventhub;
using Lobby;
using UnityEngine;

namespace App {
    public class InGameObserver : MonoBehaviour {
        public void Start() {
            AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", PlayerCompletedLevel, this);
        }

        public void PlayerCompletedLevel(object sender, EventArguments eventArguments) {
            foreach (var player in FindObjectsOfType<LobbyPlayer>()) {
//                if (player.playerName == eventArguments.Message) {
                    Debug.Log(player.playerName);
//                }
            }
        }
    }
}