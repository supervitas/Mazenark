using App.EventSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace App {
    public class AppManager : MonoBehaviour {
        public static AppManager Instance { get; private set; }
        public MazeSizeGenerator MazeSize { get; private set; }      
        public MazeBuilder.MazeBuilder MazeInstance { get; set; }
        public Publisher EventHub { get; private set; }

        //Singletone which starts firstly then other scripts;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                CommonSetUp();
            }
        }

        public void CommonSetUp() {
            EventHub = new Publisher();
            MazeSize = new MazeSizeGenerator();
        }


        public GameObject InstantiateSOC(GameObject go, Vector3 position, Quaternion rotation) { //ServerOrClient
            var instantiated = Instantiate(go, position, rotation);
//            NetworkServer.Spawn(instantiated);
            return instantiated;
        }

    }
}
