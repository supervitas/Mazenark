using App.EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class EventsUi : MonoBehaviour {
        public Canvas CanvasObject;
        // Use this for initialization

        void Start () {
            App.AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", OnLevelComplete, this);
            App.AppManager.Instance.EventHub.Subscribe("PlayerDied", OnPlayerDied, this);
        }

        private void OnDestroy() {
            App.AppManager.Instance.EventHub.UnsubscribeFromAll(this);
        }

        private void OnLevelComplete(object sender, EventArguments args) {
            CanvasObject.enabled = true;
            Text t = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            t.text = string.Format("{0} finished game", args.Message);
            t.color = Color.green;
            Invoke("TurnOffCanvas", 3);
        }
        private void OnPlayerDied(object sender, EventArguments args) {
            CanvasObject.enabled = true;
            Text t = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            t.text = string.Format("{0} died", args.Message);
            t.color = Color.red;
            Invoke("TurnOffCanvas", 3);
        }


        private void TurnOffCanvas() {
            CanvasObject.enabled = false;
        }
    }
}
