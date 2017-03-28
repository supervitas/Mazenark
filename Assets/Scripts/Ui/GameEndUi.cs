using App.EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui {
    public class GameEndUi : MonoBehaviour {
        public Canvas CanvasObject;
        // Use this for initialization
        void Start () {
            App.AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", onLevelComplete, this);
        }

        void onLevelComplete(object sender, EventArguments args) {
            CanvasObject.GetComponent<Canvas> ().enabled = true;
//            SceneManager. (Application.loadedLevel);

        }
    }
}
