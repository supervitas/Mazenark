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
        public Dictionary<Biome, Coordinate> BiomesSize = new Dictionary<Biome, Coordinate>();

        public Maze (int width = 10, int height = 10) {
            if (width < 5)
                width = 5;
            if (height < 5)
                height = 5;

            tiles = new Tile[width, height];

            for (var i = 0; i < width; i++)
				for (var j = 0; j < height; j++)
					tiles[i, j] = new Tile(new Coordinate(i, j));

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

        public int Width {
            get { return tiles.GetLength(0); }
        }

        public int Height {
            get { return tiles.GetLength(1); }
        }

		public Room AsRoom {
			get;  private set;
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
	}
}