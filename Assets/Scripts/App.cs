using System.Collections;
using System.Collections.Generic;
using MazeBuilder;
using UnityEngine;

public class App : MonoBehaviour {
    public static App Instance;
    public MazeSizeGenerator MazeSize;

	//Singletone which starts firstly then other scripts;
    private void Awake () {
        if (Instance == null) {
            Instance = this;
        }
        SetUp();
    }

    private void SetUp() {
        MazeSize = MazeSizeGenerator.Instance;
        MazeSize.GenerateFixedSize();
    }

}
