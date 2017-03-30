using MazeBuilder.Utility;

namespace MazeBuilder.BiomeGenerators.PlacementRules {
    internal class StraightWallRule : PlacementRule {
		// Assuming mesh is outerEdge wall.
		public override bool CanPlaceSomething(Maze maze, Coordinate where, Edge whereExactly, bool occupyEdges = false) {
			// Check if self as no meshes yet.
			if (maze[where].EdgeOccupied(whereExactly)) {
				return false;
			}

			// Check if up is empty tile and right tile is wall.
			Tile tileUp;
			Edge edgeUp;
			Tile tileRight;
			Edge edgeRight;

			GetShiftedTileAndEdge(maze, where, whereExactly, Direction.Up, out tileUp, out edgeUp);
			var isUpEmpty = IsEmpty(tileUp, maze[where]);
			GetShiftedTileAndEdge(maze, where, whereExactly, Direction.Right, out tileRight, out edgeRight);
			var isRightNotEmpty = IsWall(tileRight, edgeRight, maze[where]);

			if (isUpEmpty && isRightNotEmpty) {
				// Check if up right tile is empty.
				Tile tileUpRight;
				Edge edgeUpRight;
				GetShiftedTileAndEdge(maze, tileRight.Position, edgeRight, Direction.Right, out tileUpRight, out edgeUpRight);
				var isUpRightEmpty = IsEmpty(tileUpRight, maze[where]);

				if (isUpRightEmpty) {
					if (occupyEdges) {
						maze[where].OccupyEdge(whereExactly);
						tileRight.OccupyEdge(edgeRight);
					}
					return true;
				}
			}
			return false;
		}
	}
}
