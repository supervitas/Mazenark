using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public bool Equals(Coordinate other) {
			// If parameter is null return false:
			if ((object) other == null) {
				return false;
			}

			// Return true if the fields match:
			return (X == other.X) && (Y == other.Y);
		}
	}
}
