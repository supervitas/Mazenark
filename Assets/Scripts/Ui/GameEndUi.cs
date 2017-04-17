using App.EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui {
    public class GameEndUi : MonoBehaviour {
        public Canvas CanvasObject;
        // Use this for initialization
        private Canvas _canvas;
        void Start () {
            App.AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", OnLevelComplete, this);
            App.AppManager.Instance.EventHub.Subscribe("PlayerDead", OnPlayerDead, this);
            _canvas = CanvasObject.GetComponent<Canvas>();
        }

        private void OnLevelComplete(object sender, EventArguments args) {
            _canvas.enabled = true;
            Text t = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            t.text = string.Format("{0} Reached Safehouse", args.Message);
            Invoke("TurnOffCanvas", 3);
//            SceneManager. (Application.loadedLevel);
        }

        private void OnPlayerDead(object sender, EventArguments args) {
            _canvas.enabled = true;
            Text t = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            t.text = "You Failed";
        }

        private void TurnOffCanvas() {
            _canvas.enabled = false;
        }
    }
}
