using System;
using System.Collections.Generic;

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
        get{
            return importantPlaces;
        }
    }

	public int Width {
		get { return tiles.GetLength(0); }
	}

	public int Height {
		get { return tiles.GetLength(1); }
	}

	public class Coordinate {
        public Coordinate(int x, int y) {
            X = x;
            Y = y;
        }

        public int X { get; private set; }

        public int Y { get; private set; }
    }

	public class Room {
		private Coordinate topLeft;
		private Coordinate bottomRight;

		public Room(Coordinate topLeft, Coordinate bottomRight) {
			this.topLeft = topLeft;
			this.bottomRight = bottomRight;
		}

		public Room(int topLeftXCoordinate, int topLeftYCoordinate, int bottomRightXCoordinate, int bottomRightYCoordinate) {
			int lesserX = topLeftXCoordinate < bottomRightXCoordinate ? topLeftXCoordinate : bottomRightXCoordinate;
			int greaterX = topLeftXCoordinate < bottomRightXCoordinate ? bottomRightXCoordinate : topLeftXCoordinate;
			int lesserY = topLeftYCoordinate < bottomRightYCoordinate ? topLeftYCoordinate : bottomRightYCoordinate;
			int greaterY = topLeftYCoordinate < bottomRightYCoordinate ? bottomRightYCoordinate : topLeftYCoordinate;

			topLeft = new Coordinate(lesserX, lesserY);
			bottomRight = new Coordinate(greaterX, greaterY);
		}

		public Coordinate TopLeftCorner {
			get {
				return topLeft;
				}
		}

		public Coordinate TopRightCorner {
			get {
				return new Coordinate(bottomRight.X, topLeft.Y);
			}
		}

		public Coordinate BottomRightCorner {
			get {
				return bottomRight;
			}
		}

		public Coordinate BottomLeftCorner {
			get {
				return new Coordinate(topLeft.X, bottomRight.Y);
			}
		}

		// Will return top-left coordinate of four center squares if room has even sides.
		public Coordinate Center {
			get {
				return new Coordinate((topLeft.X + bottomRight.X) / 2, (topLeft.Y + bottomRight.Y) / 2);
			}
		}

		public int Width {
			get {
				return bottomRight.X - topLeft.X;
			}
		}

		public int Height {
			get {
				return bottomRight.Y - topLeft.Y;
			}
		}
	}

}




