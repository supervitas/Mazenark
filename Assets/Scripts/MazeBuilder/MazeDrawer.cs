using System;
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
        public GameObject Spawn;
        public GameObject SafeHouse;
        public GameObject Water;
        public GameObject Earth;
        public GameObject Fire;
        public GameObject Wind;

        public GameObject prefab_floor; //also need to do something to floor
        // Use this for initialization

        // Update is called once per frame
        private void Start() {
            var mazeSize = MazeSizeGenerator.Instance; // TODO move initilize to class of application? Google script execution order
            mazeSize.GenerateFixedSize();
//            MakeSharedMaterialColors(); // This will set up Colors to materials forever
            var maze = new MazeBuilder(mazeSize.X, mazeSize.Y).Maze;

            for (var i = 0; i < maze.Tiles.GetLength(0); i++) {
                var biomeBatches = new Dictionary<Biome, GameObject>();
                for (var j = 0; j < maze.Tiles.GetLength(1); j++) {
                    var y = maze.Tiles[i, j].type == Tile.Type.Wall ? 0 : -Constants.Maze.TILE_SIZE / 2 + 0.1f;

                    var cube = Instantiate(maze.Tiles[i, j].type == Tile.Type.Wall ?
                        GetCubeByType(maze.Tiles[i, j].biome) : prefab_floor, new Vector3(TransformToWorldCoordinate(i), y, TransformToWorldCoordinate(j)), Quaternion.identity);
                    var renderer = cube.GetComponent<Renderer>();

                    if (maze.Tiles[i, j].type != Tile.Type.Wall) { // temp fix while no gameobects to floor
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

                    if (maze.Tiles[i, j].type == Tile.Type.Wall) {
                        if (!biomeBatches.ContainsKey(maze.Tiles[i, j].biome)) {
                            biomeBatches.Add(maze.Tiles[i, j].biome,
                                new GameObject {name = "Grouped Biomes", isStatic = true});
                        }
                        cube.transform.parent = biomeBatches[maze.Tiles[i, j].biome].transform;

                    }
                }
                foreach (var batch in biomeBatches.Values) {
                    StaticBatchingUtility.Combine(batch.gameObject);
                }

            }
        }

        private GameObject GetCubeByType(Biome biome) {
            if (biome == Biome.Spawn) {
                return Spawn;
            }
            if (biome == Biome.Safehouse) {
                return SafeHouse;
            }
            if (biome == Biome.Water) {
                return Water;
            }
            if (biome == Biome.Earth) {
                return Earth;
            }
            if (biome == Biome.Fire) {
                return Fire;
            }
            if (biome == Biome.Wind) {
                return Wind;
            }

            return Earth; //default return
        }

        private void MakeSharedMaterialColors() {
            Spawn.GetComponent<Renderer>().sharedMaterial.color = SpawnBiomeColor;
            SafeHouse.GetComponent<Renderer>().sharedMaterial.color = SafehouseBiomeColor;
            Water.GetComponent<Renderer>().sharedMaterial.color = WaterBiomeColor;
            Earth.GetComponent<Renderer>().sharedMaterial.color = EarthBiomeColor;
            Fire.GetComponent<Renderer>().sharedMaterial.color = FireBiomeColor;
            Wind.GetComponent<Renderer>().sharedMaterial.color = WindBiomeColor;
        }


        private void Update(){}

        // E.g. 0 → 4.5, 3 → 3*9 + 4.5
        private float TransformToWorldCoordinate(int absoluteCoordinate) {
            return absoluteCoordinate * Constants.Maze.TILE_SIZE + Constants.Maze.TILE_SIZE / 2.0f;
        }

    }
}
