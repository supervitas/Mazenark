using System;

namespace MazeBuilder.Utility {
	public class Coordinate {
		public Coordinate(int x, int y) {
			X = x;
			Y = y;
		}


		public int X { get; set; }

		public int Y { get; set; }

		public int ManhattanDistanceTo(Coordinate other) {
			return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
		}

		public int EuclidianDistanceTo(Coordinate other) {
			return (int) Math.Round(Math.Sqrt( Math.Abs(X - other.X) * Math.Abs(X - other.X) + Math.Abs(Y - other.Y) * Math.Abs(Y - other.Y)));
		}

		public override bool Equals(object obj) {
			Coordinate other = obj as Coordinate;

			if (other == null)
				return false;

			return (X == other.X) && (Y == other.Y);
		}
		
		public override int GetHashCode() {
			int hash = 37;

			hash = hash * 13 + X.GetHashCode();
			hash = hash * 13 + Y.GetHashCode();

			return hash;
		}

	    public override string ToString() {
	        return string.Format("x = {0}, y = {1}", X, Y);
	    }
	}
}
