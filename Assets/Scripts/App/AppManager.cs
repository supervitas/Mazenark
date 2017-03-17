using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.EventSystem;
using UnityEngine;

namespace App {
    public class AppManager : MonoBehaviour {
        public static AppManager Instance;
        public MazeSizeGenerator MazeSize { get; private set; }
        public MazeBuilder.MazeBuilder MazeInstance { get; private set; }
        public Publisher EventHub;

        //Singletone which starts firstly then other scripts;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            SetUp();
        }

        private void SetUp() {
            EventHub = new Publisher();
            MazeSize = new MazeSizeGenerator();
            MazeSize.GenerateFixedSize();
            MazeInstance = new MazeBuilder.MazeBuilder(MazeSize.X, MazeSize.Y);
        }

    }
}
