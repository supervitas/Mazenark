using System.Collections.Generic;
using MazeBuilder.Utility;

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

        public Maze (int width = 10, int height = 10) {
            if (width < 5)
                width = 5;
            if (height < 5)
                height = 5;

            tiles = new Tile[width, height];

            for (var i = 0; i < width; i++)
				for (var j = 0; j < height; j++)
					tiles[i, j] = new Tile();
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

        public void CutWalls(Coordinate topLeft, Coordinate bottomRight) {
            CutWalls(new Room(topLeft, bottomRight));
        }

        public void CutWalls(Room room, Biome fillWith = null) {
            for (var i = room.TopLeftCorner.X; i <= room.TopRightCorner.X; i++)
				for (var j = room.TopLeftCorner.Y; j <= room.BottomLeftCorner.Y; j++) {
					tiles[i, j].type = Tile.Type.Empty;
					if (fillWith != null)
						tiles[i, j].biome = fillWith;
				}
        }      

    }
}




