using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeBuilder.Utility;

namespace MazeBuilder.BiomeGenerators {
	class TestRule : PlacementRule {
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
			var isUpEmpty = (tile == null || tile.Type == Tile.Variant.Empty);
			IsRequestedEdgeEmpty(maze, where, whereExactly, Direction.Right, out tile, out unused2);
			var isRightEmpty = (tile == null || tile.Type == Tile.Variant.Empty);

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
