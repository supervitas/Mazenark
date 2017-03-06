using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeBuilder.Utility {
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

		private bool IsCoordinateLiesWithin(int x, int y) {
			return IsCoordinateLiesWithin(new Coordinate(x, y));
		}

		private bool IsCoordinateLiesWithin(Coordinate point) {
			return topLeft.X <= point.X && topLeft.Y <= point.Y && bottomRight.X >= point.X && bottomRight.Y >= point.Y;
		}

		public bool IntersectsRoomAndOneTileMargin(Room anotherRoom) {
			bool doIntersect = false;
			// ±1 because of Minkovsky. Just provides beforementioned margin of 1 tile.
			int x = topLeft.X - 1;
			int y = topLeft.Y - 1;
			int xRight = bottomRight.X + 1;
			int yBottom = bottomRight.Y + 1;

			// if one of the points above lies within another room, they intersect each other.
			doIntersect = IsCoordinateLiesWithin(x, y) || IsCoordinateLiesWithin(x, yBottom)
						  || IsCoordinateLiesWithin(xRight, y) || IsCoordinateLiesWithin(xRight, yBottom);

			return doIntersect;
		}
	}
}
