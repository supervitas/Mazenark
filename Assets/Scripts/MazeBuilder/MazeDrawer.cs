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
            var maze = App.AppManager.Instance.MazeInstance.Maze;

            for (var i = 0; i < maze.Width; i++) {
                var biomeGroups = new Dictionary<Biome, List<GameObject>>();
                var floorGroups = new Dictionary<Biome, List<GameObject>>();
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
                        if (!biomeGroups.ContainsKey(maze.Tiles[i, j].Biome)) {
                            biomeGroups.Add(maze.Tiles[i, j].Biome, new List<GameObject>());
                        }
                        biomeGroups[maze.Tiles[i, j].Biome].Add(tile);
                    } else {
                        if (!floorGroups.ContainsKey(maze.Tiles[i, j].Biome)) {
                            floorGroups.Add(maze.Tiles[i, j].Biome, new List<GameObject>());
                        }
                        floorGroups[maze.Tiles[i, j].Biome].Add(tile);
                    }
                }
                foreach (var biome in biomeGroups.Values) {
//                    AppManager.Batcher.Instance.BatchByDivider(5, biome.ToArray(), "test");
                }
                foreach (var biome in floorGroups.Values) {
//                    AppManager.Batcher.Instance.BatchByDivider(5, biome.ToArray(), "floor");
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
