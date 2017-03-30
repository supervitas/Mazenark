using MazeBuilder.Utility;

namespace MazeBuilder.BiomeGenerators.PlacementRules {
	class InnerEdgeRule : PlacementRule {
		// Assuming mesh is outerEdge wall.
		public override bool CanPlaceSomething(Maze maze, Coordinate where, Edge whereExactly, bool occupyEdges = false) {
			// Check if self as no meshes yet.
			if (maze[where].EdgeOccupied(whereExactly)) {
				return false;
			}

			// Check if up and right tiles are walls.
			Tile tileUp;
			Edge edgeUp;
			Tile tileRight;
			Edge edgeRight;

			GetShiftedTileAndEdge(maze, where, whereExactly, Direction.Up, out tileUp, out edgeUp);
			var isUpNotEmpty = IsWall(tileUp, edgeUp, maze[where]);
			GetShiftedTileAndEdge(maze, where, whereExactly, Direction.Right, out tileRight, out edgeRight);
			var isRightNotEmpty = IsWall(tileRight, edgeRight, maze[where]);

			if (isUpNotEmpty && isRightNotEmpty) {
				if (occupyEdges) {
					maze[where].OccupyEdge(whereExactly);
					tileRight.OccupyEdge(edgeRight);
					tileUp.OccupyEdge(edgeUp);
				}
				return true;
			} else {
				return false;
			}
		}
	}
}
