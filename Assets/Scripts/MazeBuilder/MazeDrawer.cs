using System.Collections;
using System.Collections.Generic;
using Constants;
using Walls;
using UnityEngine;

public class MazeDrawer : MonoBehaviour {

    public const int TILE_SIZE = 9; // should be 8, but I want some gaps between cubes.
    public static Color SPAWN_BIOME_COLOR = new Color(1, 1, 1, 0.25f);
    public static Color SAFEHOUSE_BIOME_COLOR = new Color(1, 1, 1, 0.75f);
    public static Color WATER_BIOME_COLOR = new Color(0, 0.1f, 1);
    public static Color EARTH_BIOME_COLOR = new Color(1, 0.7f, 0);
    public static Color FIRE_BIOME_COLOR = new Color(0.7f, 1, 0);
    public static Color WIND_BIOME_COLOR = new Color(0.5f, 0.5f, 0.5f, 0.9f);

    [Tooltip("Object to be spawned as maze blocks")]
    public GameObject prefab;
    // Use this for initialization
    private void Start() {
        var mazeSize = MazeSizeGenerator.Instance; // TODO move initilize to class of application? Google script execution order
        mazeSize.generateFixedSize();
        var maze = new MazeBuilder(mazeSize.X, mazeSize.Y).Maze;
        for (var i = 0; i < maze.Tiles.GetLength(0); i++)
            for (var j = 0; j < maze.Tiles.GetLength(1); j++) {
                var y = maze.Tiles[i, j].type == Tile.Type.Wall ? 0 : -TILE_SIZE;
                
                var cube = Instantiate(prefab, new Vector3(TransformToWorldCoordinate(i), y,
                    TransformToWorldCoordinate(j)), Quaternion.identity);
                var renderer = cube.GetComponent<Renderer>();

                if (renderer == null) continue;
                if (maze.Tiles[i, j].biome == Biome.Spawn)
                    renderer.material.color = SPAWN_BIOME_COLOR;
                if (maze.Tiles[i, j].biome == Biome.Safehouse)
                    renderer.material.color = SAFEHOUSE_BIOME_COLOR;
                if (maze.Tiles[i, j].biome == Biome.Water)
                    renderer.material.color = WATER_BIOME_COLOR;
                if (maze.Tiles[i, j].biome == Biome.Earth)
                    renderer.material.color = EARTH_BIOME_COLOR;
                if (maze.Tiles[i, j].biome == Biome.Fire)
                    renderer.material.color = FIRE_BIOME_COLOR;
                if (maze.Tiles[i, j].biome == Biome.Wind)
                    renderer.material.color = WIND_BIOME_COLOR;
            }
    }

    // Update is called once per frame
    private void Update(){}

    // E.g. 0 → 4.5, 3 → 3*9 + 4.5
    private float TransformToWorldCoordinate(int absoluteCoordinate) {
        return absoluteCoordinate * TILE_SIZE + TILE_SIZE / 2.0f;
    }
}
