using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDrawer : MonoBehaviour {

    public const int TILE_SIZE = 9; // should be 8, but I want some gaps between cubes.

    [Tooltip("Object to be spawned as maze blocks")]
    public GameObject prefab;
    // Use this for initialization
    void Start()
    {
        Maze maze = new MazeBuilder(64, 64).Maze;
        for (int i = 0; i < maze.Tiles.GetLength(0); i++)
            for (int j = 0; j < maze.Tiles.GetLength(1); j++)
                Instantiate(prefab, new Vector3(TransformToWorldCoordinate(i), 0, TransformToWorldCoordinate(j)), Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
		
	}

    // E.g. 0 → 4.5, 3 → 3*9 + 4.5
    private float TransformToWorldCoordinate(int absoluteCoordinate)
    {
        return absoluteCoordinate * TILE_SIZE + TILE_SIZE / 2.0f;
    }
}
