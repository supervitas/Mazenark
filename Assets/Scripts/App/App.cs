using MazeBuilder;
using UnityEngine;

namespace App {
    public class App : MonoBehaviour {
        public static App Instance;
        public MazeSizeGenerator MazeSize { get; private set; }

        //Singletone which starts firstly then other scripts;
        private void Awake () {
            if (Instance == null) {
                Instance = this;
            }
            SetUp();
        }

        private void SetUp() {
            MazeSize = new MazeSizeGenerator();
            MazeSize.GenerateFixedSize();
        }

    }
}
