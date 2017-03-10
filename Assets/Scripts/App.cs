using System.Collections;
using System.Collections.Generic;
using MazeBuilder;
using UnityEngine;

public class App : MonoBehaviour {

	//Singletone which starts firstly then other scripts;
    private void Start () {
	    var mazeSize = MazeSizeGenerator.Instance;
	    mazeSize.GenerateFixedSize();
	}
	
	// Update is called once per frame
    private void Update () {}
}
