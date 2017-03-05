using System.Collections.Generic;
using UnityEngine;

namespace MazeBuilder {
    public class MazeDrawer : MonoBehaviour {

        public static Color SpawnBiomeColor = new Color(1, 1, 1, 0.25f);
        public static Color SafehouseBiomeColor = new Color(1, 1, 1, 0.75f);
        public static Color WaterBiomeColor = new Color(0, 0.1f, 1);
        public static Color EarthBiomeColor = new Color(1, 0.7f, 0);
        public static Color FireBiomeColor = new Color(0.7f, 1, 0);
        public static Color WindBiomeColor = new Color(0.5f, 0.5f, 0.5f, 0.9f);

        [Tooltip("Object to be spawned as maze blocks")]
        public GameObject prefab;
        public GameObject prefab_floor;
        // Use this for initialization

        // Update is called once per frame
        private void Start() {
            var mazeSize = MazeSizeGenerator.Instance; // TODO move initilize to class of application? Google script execution order
            mazeSize.GenerateFixedSize();
            var maze = new MazeBuilder(mazeSize.X, mazeSize.Y).Maze;
            for (var i = 0; i < maze.Tiles.GetLength(0); i++) {
                for (var j = 0; j < maze.Tiles.GetLength(1); j++) {
                    var y = maze.Tiles[i, j].type == Tile.Type.Wall ? 0 : -Constants.Maze.TILE_SIZE / 2 + 0.1f;
                    GameObject cube;
                    if (maze.Tiles[i, j].type == Tile.Type.Wall)
                        cube = Instantiate(prefab,
                            new Vector3(TransformToWorldCoordinate(i), y, TransformToWorldCoordinate(j)),
                            Quaternion.identity);
                    else
                        cube = Instantiate(prefab_floor,
                            new Vector3(TransformToWorldCoordinate(i), y, TransformToWorldCoordinate(j)),
                            Quaternion.identity);

                    var renderer = cube.GetComponent<Renderer>();

                    if (renderer == null) continue;
                    if (maze.Tiles[i, j].biome == Biome.Spawn)
                        renderer.material.color = SpawnBiomeColor;
                    if (maze.Tiles[i, j].biome == Biome.Safehouse)
                        renderer.material.color = SafehouseBiomeColor;
                    if (maze.Tiles[i, j].biome == Biome.Water)
                        renderer.material.color = WaterBiomeColor;
                    if (maze.Tiles[i, j].biome == Biome.Earth)
                        renderer.material.color = EarthBiomeColor;
                    if (maze.Tiles[i, j].biome == Biome.Fire)
                        renderer.material.color = FireBiomeColor;
                    if (maze.Tiles[i, j].biome == Biome.Wind)
                        renderer.material.color = WindBiomeColor;
                }
            }
//            StaticBatchingUtility.Combine(gos, Root);
        }

        private void Update(){}

        // E.g. 0 → 4.5, 3 → 3*9 + 4.5
        private float TransformToWorldCoordinate(int absoluteCoordinate) {
            return absoluteCoordinate * Constants.Maze.TILE_SIZE + Constants.Maze.TILE_SIZE / 2.0f;
        }
    }
}
