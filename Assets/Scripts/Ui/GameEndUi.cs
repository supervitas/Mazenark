using App.EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui {
    public class GameEndUi : MonoBehaviour {
        public Canvas CanvasObject;
        // Use this for initialization

        void Start () {
            App.AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", OnLevelComplete, this);
            App.AppManager.Instance.EventHub.Subscribe("PlayerDead", OnPlayerDead, this);

        }

        private void OnDestroy() {
            App.AppManager.Instance.EventHub.UnsubscribeFromAll(this);
        }

        private void OnLevelComplete(object sender, EventArguments args) {
            CanvasObject.enabled = true;
            Text t = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            t.text = string.Format("{0} Reached Safehouse", args.Message);
            Invoke("TurnOffCanvas", 3);
//            SceneManager. (Application.loadedLevel);
        }

        private void OnPlayerDead(object sender, EventArguments args) {
            CanvasObject.enabled = true;
            Text t = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            t.text = "You Failed";
        }

        private void TurnOffCanvas() {
            CanvasObject.enabled = false;
        }
    }
}
