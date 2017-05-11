using App.Eventhub;
using UnityEngine;

namespace App {
    public class InGameObserver : MonoBehaviour {
        public void Start() {
            AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", PlayerCompletedLevel, this);
        }

        public void PlayerCompletedLevel(object sender, EventArguments eventArguments) {
            Debug.Log(eventArguments.Message);
        }
    }
}