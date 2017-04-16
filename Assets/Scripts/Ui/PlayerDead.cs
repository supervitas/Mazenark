using System;
using App.EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui {
    public class PlayerDead : MonoBehaviour {
        public Canvas CanvasObject;

        private Canvas _canvas;

        private void Start () {
            _canvas = CanvasObject.GetComponent<Canvas>();
            App.AppManager.Instance.EventHub.Subscribe("PlayerDied", OnPlayerDied, this);
        }

        private void OnPlayerDied(object sender, EventArguments args) {
            _canvas.enabled = true;
            Text t = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            t.text = string.Format("{0} died", args.Message);
            Invoke("TurnOffCanvas", 3);
        }

        private void TurnOffCanvas() {
            _canvas.enabled = false;
        }
    }
}
