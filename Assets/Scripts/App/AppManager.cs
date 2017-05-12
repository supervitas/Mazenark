using App.Eventhub;
using MazeBuilder.Utility;
using UnityEngine;

namespace App {
    public class AppManager : MonoBehaviour {
        public static AppManager Instance { get; private set; }
        public MazeSizeGenerator MazeSize { get; private set; }
        public MazeBuilder.MazeBuilder MazeInstance { get; set; }
        public Publisher EventHub { get; private set; }
        public bool IsSinglePlayer = false;

        private Camera _cam;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                CommonSetUp();
            }
        }

        public void CommonSetUp() { // also used from LobbyManager to remove all eventhandlers registered in previous game by reiniting
            EventHub = new Publisher();
            MazeSize = new MazeSizeGenerator();
            MazeInstance = null;
            IsSinglePlayer = false;
        }


        public void TurnOffAndSetupMainCamera() { // Here gameover camera will be set
            Camera.main.transform.position = Utils.TransformToWorldCoordinate(new Coordinate(
                MazeInstance.Height / 2 - 1, MazeInstance.Width / 2 - 2), 10.5f);
            _cam = Camera.main;
            _cam.enabled = false;
        }

        public void TurnOnMainCamera() {
            if (_cam) {
                _cam.enabled = true;
            }
        }
    }
}
