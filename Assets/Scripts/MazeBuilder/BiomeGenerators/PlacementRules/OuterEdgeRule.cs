using MazeBuilder.Utility;

namespace MazeBuilder.BiomeGenerators.PlacementRules {
    internal class OuterEdgeRule : PlacementRule {
		// Assuming mesh is outerEdge wall.
		public override bool CanPlaceSomething(Maze maze, Coordinate where, Edge whereExactly, bool occupyEdges = false) {
			// Check if self as no meshes yet.
			if (maze[where].EdgeOccupied(whereExactly)) {
				return false;
			}

			// Check if up and right tiles are passages.
			Tile tile;
			Edge unused2;

			GetShiftedTileAndEdge(maze, where, whereExactly, Direction.Up, out tile, out unused2);
			var isUpEmpty = IsEmpty(tile, maze[where]);
			GetShiftedTileAndEdge(maze, where, whereExactly, Direction.Right, out tile, out unused2);
			var isRightEmpty = IsEmpty(tile, maze[where]);

			if (isUpEmpty && isRightEmpty) {
				if (occupyEdges) {
					maze[where].OccupyEdge(whereExactly);
				}
				return true;
			} else {
				return false;
			}
		}
	}
}
