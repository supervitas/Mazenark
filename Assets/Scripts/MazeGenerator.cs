using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

    [Tooltip("Object to be spawned as maze blocks")]
    public GameObject prefab;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 10; i++)
            Instantiate(prefab, new Vector3(i * 10.0f, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
