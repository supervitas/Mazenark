using MazeBuilder.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeBuilder {
    public class MazeDrawer : MonoBehaviour {
        [Tooltip("Object to be spawned as maze blocks")]

        #region BiomeWalls
        [Header("Biome Walls")]
        public GameObject Spawn;
        public GameObject SafeHouse;
        public GameObject Water;
        public GameObject Earth;
        public GameObject Fire;
        public GameObject Wind;
        #endregion

        #region BiomeFloors
        [Header("Biome Floors")]
        public GameObject PrefabFloor;
        public GameObject WaterFloor;
        #endregion

        #region BiomeCubeGenerators
        [Header("Biome Cube Generators")]
		public CubeGenerator EarthCubeGenerator;
        #endregion

		private void Start() {
            var mazeSize = App.Instance.MazeSize;
            var maze = new MazeBuilder(mazeSize.X, mazeSize.Y).Maze;

            for (var i = 0; i < maze.Width; i++) {
                var biomeBatches = new Dictionary<Biome, GameObject>();
                var floorBiomeBatches = new Dictionary<Biome, GameObject>();
                for (var j = 0; j < maze.Height; j++) {
					var coordinate = new Vector3(TransformToWorldCoordinate(i),
					    GetYForTile(maze.Tiles[i, j].type, maze.Tiles[i, j].Biome ), TransformToWorldCoordinate(j));
					GameObject tile;

                    if (HasGenerator(maze.Tiles[i, j].Biome) && maze.Tiles[i, j].type == Tile.Type.Wall) {
                        tile = EarthCubeGenerator.Create(maze.Tiles[i, j].Biome, new Coordinate(i, j), maze,
                            coordinate);
                    } else {
                        tile = Instantiate(
                            maze.Tiles[i, j].type == Tile.Type.Wall ?
                                GetCubeByType(maze.Tiles[i, j].Biome) :
                                GetFloorByType(maze.Tiles[i, j].Biome), coordinate, Quaternion.identity);
                    }


                    if (maze.Tiles[i, j].type == Tile.Type.Wall) {
                        if (!biomeBatches.ContainsKey(maze.Tiles[i, j].Biome)) {
                            biomeBatches.Add(maze.Tiles[i, j].Biome,
                                new GameObject {name = "Grouped Biomes", isStatic = true});
                        }
                        tile.transform.parent = biomeBatches[maze.Tiles[i, j].Biome].transform;
                    } else {
                        if (!floorBiomeBatches.ContainsKey(maze.Tiles[i, j].Biome)) {
                            floorBiomeBatches.Add(maze.Tiles[i, j].Biome,
                                new GameObject {name = "Floor Grouped", isStatic = true});
                        }
                        tile.transform.parent = floorBiomeBatches[maze.Tiles[i, j].Biome].transform;
                    }
                }
				
                foreach (var batch in biomeBatches.Values) {
//                    StaticBatchingUtility.Combine(batch.gameObject);
                }

			
            }
        }

		private bool HasGenerator(Biome biome) {
			if (biome == Biome.Spawn) {
				return false;
			}
			if (biome == Biome.Safehouse) {
				return false;
			}
			if (biome == Biome.Water) {
				return false;
			}
			if (biome == Biome.Earth) {
				return true;
			}
			if (biome == Biome.Fire) {
				return false;
			}
			if (biome == Biome.Wind) {
				return false;
			}
			return false;
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
        private GameObject GetFloorByType(Biome biome) {
            if (biome == Biome.Water) {
                return WaterFloor;
            }

            return PrefabFloor; //default return
        }

        private float GetYForTile(Tile.Type type, Biome biome) {
            return type == Tile.Type.Wall ? biome.WallYCoordinate : biome.FloorYCoordinate;
        }

        private void Update() {}

        // E.g. 0 → 4.5, 3 → 3*9 + 4.5
        private float TransformToWorldCoordinate(int absoluteCoordinate) {
            return absoluteCoordinate * Constants.Maze.TILE_SIZE + Constants.Maze.TILE_SIZE / 2.0f;
        }

    }
}
