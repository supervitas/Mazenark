using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeBuilder.Utility;

namespace MazeBuilder.BiomeGenerators {
	class TestRule : PlacementRule {
		// Assuming mesh is outerEdge wall.
		public override bool CanPlaceSomething(Maze maze, Coordinate where, Edge whereExactly, bool occupyEdges = false) {
			if (occupyEdges)
				maze[where].OccupyEdge(whereExactly);
			return true;
		}
	}
}
