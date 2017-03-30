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

			IsRequestedEdgeEmpty(maze, where, whereExactly, Direction.Up, out tile, out unused2);
			var isUpEmpty = (tile == null || tile.Type != Tile.Variant.Wall || tile.Biome != maze[where].Biome);
			IsRequestedEdgeEmpty(maze, where, whereExactly, Direction.Right, out tile, out unused2);
			var isRightEmpty = (tile == null || tile.Type != Tile.Variant.Wall || tile.Biome != maze[where].Biome);

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
