﻿using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace App {
    public class AppManager : MonoBehaviour {
        public static AppManager Instance { get; private set; }
        public MazeSizeGenerator MazeSize { get; private set; }
        public MazeBuilder.MazeBuilder MazeInstance { get; set; }
        public Publisher EventHub { get; private set; }

        private Camera cam;
        //Singletone which starts firstly then other scripts;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                CommonSetUp();
            }
        }

        public void CommonSetUp() { // also used from LobbyManager to remove all eventhandlers registered in previous game by reiniting
            EventHub = new Publisher();
            MazeSize = new MazeSizeGenerator();
        }


        public GameObject InstantiateSOC(GameObject go, Vector3 position, Quaternion rotation) { //ServerOrClient
            var instantiated = Instantiate(go, position, rotation);
//            NetworkServer.Spawn(instantiated);
            return instantiated;
        }

        public void TurnOffAndSetupMainCamera() {
            Camera.main.transform.position = Utils.TransformToWorldCoordinate(new Coordinate(
                MazeInstance.Height / 2 - 2, MazeInstance.Width / 2 - 2));
            cam = Camera.main;
            cam.enabled = false;
        }

        public void TurnOnMainCamera() {
            cam.enabled = true;
        }
    }
}
