using System.Collections.Generic;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder {
    public class Maze {
        // First index is x-coordinate, second index is y-coordinate.
        // [0, 0] stands for topmost leftmost square.
        // 0----5----10---→ x
        // |
        // |
        // ↓
        // y
        private Tile[,] tiles;
		private List<Coordinate> importantPlaces = new List<Coordinate>();  // Should have at least one path leading to them.
        private List<Room> rooms = new List<Room>();
		private List<Room> spawns = new List<Room>();
		public Dictionary<Biome, Coordinate> BiomesSize = new Dictionary<Biome, Coordinate>();
		private List<TileCollection> biomeList = null;
		public int MaxBiomeID { get; set; }


        public Maze (int width = 10, int height = 10, bool fromServer = false) {
            if (width < 5)
                width = 5;
            if (height < 5)
                height = 5;

            tiles = new Tile[width, height];

            for (var i = 0; i < width; i++)
				for (var j = 0; j < height; j++)
					tiles[i, j] = new Tile(new Coordinate(i, j));

            if (fromServer) {
                return;
            }

			AsRoom = new Room(new Coordinate(0, 0), new Coordinate(width - 1, height - 1));
		}

        public Tile[,] Tiles {
            get {
                return tiles;
            }
        }

		public List<Coordinate> ImportantPlaces{
            get {
                return importantPlaces;
            }
        }

        public List<Room> Rooms {
            get {
                return rooms;
            }
        }
		public List<Room> Spawns {
			get {
				return spawns;
			}
		}

		public int Width {
            get { return tiles.GetLength(0); }
        }

        public int Height {
            get { return tiles.GetLength(1); }
        }

		public Room AsRoom {
			get; private set;
		}

		public List<TileCollection> Biomes {
			get { return biomeList; }
		}

		public void CutPassage(Coordinate topLeft, Coordinate bottomRight) {
            CutWalls(new Room(topLeft, bottomRight), type:Tile.Variant.Empty);
        }

		public Tile this[Coordinate point] {
			get {
				return Tiles[point.X, point.Y];
			}
			private set {
				Tiles[point.X, point.Y] = value;
			}
		}

		public Tile this[int x, int y] {
			get {
				return Tiles[x, y];
			}
			private set {
				Tiles[x, y] = value;
			}
		}

		public void CutWalls(Room room, Biome fillWith = null, Tile.Variant type = Tile.Variant.Room) {
		    if (fillWith != null && !BiomesSize.ContainsKey(fillWith)) {
		        BiomesSize.Add(fillWith, new Coordinate(0, 0));
		    }
            for (var i = room.TopLeftCorner.X; i <= room.TopRightCorner.X; i++)
				for (var j = room.TopLeftCorner.Y; j <= room.BottomLeftCorner.Y; j++) {
					tiles[i, j].Type = type;
				    if (fillWith != null) {
				        tiles[i, j].Biome = fillWith;
				    }
				}
        }

		public bool IsPointWithin(int x, int y) {
			return x >= 0 && x < Width && y >= 0 && y < Height;
		}
		public bool IsPointWithin(Coordinate point) {
			return IsPointWithin(point.X, point.Y);
		}
		public List<Tile> GetTilesWithinRoom(Room room) {
			var tiles = new List<Tile>();

			for (int i = room.TopLeftCorner.X; i <= room.TopRightCorner.X; i++) {
				for (int j = room.TopLeftCorner.Y; j <= room.BottomLeftCorner.Y; j++) {
					if (IsPointWithin(i, j)) {
						tiles.Add(Tiles[i, j]);
					}
				}
			}

			return tiles;
		}

		public class TileCollection {
			public readonly Biome biome;
			public readonly List<Tile> tiles = new List<Tile>();
			public readonly List<Room> rooms = new List<Room>();

			public TileCollection(Biome biome) {
				this.biome = biome;
			}
		}

		public void GenerateBiomesList() {
			biomeList = new List<TileCollection>();
			int tmpCounter = 0;
			MaxBiomeID = 999; // There I fixed it.

			// foreach biome
			for (int i = 0; i < MaxBiomeID; i++) {
				TileCollection biome = null;

				// add appropriate tiles into that biome
				foreach (Tile tile in Tiles) {
					if (tile.BiomeID == i) {
						if (biome == null) {
							biome = new Maze.TileCollection(tile.Biome);
							Biomes.Add(biome);
						}
						biome.tiles.Add(tile);
					}
				}

				//Debug.Log(":===:");
				//Debug.Log("Here are all rooms:");
				//DebugPrintRooms();
				//Debug.Log("Adding rooms for biomes:");

				// add appropriate rooms into that biome
				foreach (Room room in Rooms) {
					var tile = this[room.Center];
					if (tile.BiomeID == i) {
						// very unlikely.
						if (biome == null) {
							biome = new Maze.TileCollection(tile.Biome);
							Biomes.Add(biome);
						}
						biome.rooms.Add(room);
						tmpCounter++;
						//DebugPrintRoom(room);
					}
				}
			}

			

			Debug.Log(string.Format("Added {0} rooms from {1} total. Maze has {2} walls.", tmpCounter, Rooms.Count, DebugCountWallTiles()));

		}

		private int DebugCountWallTiles() {
			int result = 0;
			foreach (Tile tile in tiles) {
				if (tile.Type == Tile.Variant.Wall) {
					result++;
				}
			}
			return result;
		}

		private void DebugPrintRooms() {
			foreach (Room room in Rooms)
				DebugPrintRoom(room);
		}

		private void DebugPrintRoom(Room room) {
			Debug.Log("Room@" + this[room.Center].Biome.Name + string.Format(".\n{0,2}:{1,2} {0,2}:{1,2}", room.TopLeftCorner.X, room.TopLeftCorner.Y, room.TopRightCorner.X, room.TopRightCorner.Y)
				+ string.Format("\n   {0,2}:{1,2}", room.Center.X, room.Center.Y)
				+ string.Format("\n{0,2}:{1,2} {0,2}:{1,2}", room.BottomLeftCorner.X, room.BottomLeftCorner.Y, room.BottomRightCorner.X, room.BottomRightCorner.Y));
		}
	}
}