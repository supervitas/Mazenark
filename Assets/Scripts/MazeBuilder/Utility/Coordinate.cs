using System;

namespace MazeBuilder.Utility {
	public class Coordinate {
		public Coordinate(int x, int y) {
			X = x;
			Y = y;
		}

		public int X { get; private set; }

		public int Y { get; private set; }

		public int ManhattanDistanceTo(Coordinate other) {
			return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
		}

		public int EuclidianDistanceTo(Coordinate other) {
			return (int) Math.Round(Math.Sqrt( Math.Abs(X - other.X) + Math.Abs(Y - other.Y) ));
		}

		public override bool Equals(object obj) {
			// If parameter is null return false:
			Coordinate other = obj as Coordinate;

			// Return true if the fields match:
			return (X == other.X) && (Y == other.Y);
		}
		
		public override int GetHashCode() {
			int hash = 37;

			hash = hash * 13 + X.GetHashCode();
			hash = hash * 13 + Y.GetHashCode();

			return hash;
		}
	}
}
