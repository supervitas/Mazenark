using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeBuilder.Utility {
	public class Direction {
		private static List<Direction> directions = new List<Direction>();
		public static List<Direction> Directions { get { return directions; } }

		public int DeltaX { get; private set; }
		public int DeltaY { get; private set; }
		public Direction Opposite { get; private set; }

		private Direction(int dx, int dy) {

			DeltaX = dx;
			DeltaY = dy;

			directions.Add(this);
		}

		public Coordinate Shift(Coordinate toBeShifted) {
			return new Coordinate(toBeShifted.X + DeltaX, toBeShifted.Y + DeltaY);
		}

		public override bool Equals(object obj) {
			// If parameter is null return false:
			Direction other = obj as Direction;

			if (other == null)
				return false;

			// Return true if the fields match:
			return (DeltaX == other.DeltaX) && (DeltaY == other.DeltaY);
		}

		public override int GetHashCode() {
			int hash = 37;

			hash = hash * 13 + DeltaX.GetHashCode();
			hash = hash * 13 + DeltaY.GetHashCode();

			return hash;
		}

		public static Direction Up = new Direction(0, -1);
		public static Direction Left = new Direction(-1, 0);
		public static Direction Bottom = new Direction(0, +1);
		public static Direction Right = new Direction(+1, 0);

		static Direction() {
			Up.Opposite = Bottom;
			Left.Opposite = Right;
			Bottom.Opposite = Up;
			Right.Opposite = Left;
		}
	}
}
