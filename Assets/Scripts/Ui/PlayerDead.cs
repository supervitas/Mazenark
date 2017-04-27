using System;
using App.EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui {
    public class PlayerDead : MonoBehaviour {
        public Canvas CanvasObject;



        private void Start () {
            App.AppManager.Instance.EventHub.Subscribe("PlayerDied", OnPlayerDied, this);
        }

        private void OnDestroy() {
            App.AppManager.Instance.EventHub.UnsubscribeFromAll(this);
        }

        private void OnPlayerDied(object sender, EventArguments args) {
            Debug.Log(args.Message);
            CanvasObject.enabled = true;
            Text t = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            t.text = string.Format("{0} died", args.Message);
            Invoke("TurnOffCanvas", 3);
        }

        private void TurnOffCanvas() {
            CanvasObject.enabled = false;
        }
    }
}
