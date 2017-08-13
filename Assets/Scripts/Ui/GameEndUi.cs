using System.Collections;
using System.Collections.Generic;
using App;
using App.Eventhub;
using Lobby;
using UnityEngine;

using UnityEngine.UI;

namespace Ui {
    public class GameEndUi : MonoBehaviour {
        public Canvas CanvasObject;
        private string playerName;

        private void Start () {
            AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", OnLevelComplete, this);
            AppManager.Instance.EventHub.Subscribe("PlayerDied", OnPlayerDead, this);
            playerName = AppLocalStorage.Instance.GetUserData().username;
        }

        private void OnDestroy() {
            AppManager.Instance.EventHub.UnsubscribeFromAll(this);
        }

        private void OnPlayerDead(object sender, EventArguments args) {
            if(args.Message != playerName) return;
            CanvasObject.enabled = true;
            Text t = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            t.text = "You Died";
            t.color = Color.red;
            Invoke(nameof(GoBackToMenu), 5);
        }

        private void OnLevelComplete(object sender, EventArguments args) {
            if (args.Message != playerName) return;
            CanvasObject.enabled = true;
            Text t = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            t.text = "Success! Take this award";
            t.color = Color.green;
            Invoke(nameof(GoBackToMenu), 5);
        }

        private void GoBackToMenu() {
            FindObjectOfType<LobbyManager>().GoBackButton();
        }

        private void TurnOffCanvas() {
            CanvasObject.enabled = false;
        }
    }
}
