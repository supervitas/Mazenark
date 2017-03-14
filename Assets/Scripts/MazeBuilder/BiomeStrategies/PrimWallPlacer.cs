using System;
using MazeBuilder.Utility;
using Random = System.Random;
using System.Collections.Generic;
using System.Diagnostics;

namespace MazeBuilder.BiomeStrategies {
    public class PrimWallPlacer : IWallPlacer {
        private static PrimWallPlacer instance = new PrimWallPlacer();
        private static Random random = new Random();

        private PrimWallPlacer() { }

        public static IWallPlacer Instance {
            get {
                return instance;
            }
        }

		public Maze PlaceWalls(Maze roomedMaze) {
			int maxIterations = roomedMaze.Width * roomedMaze.Height;
			for (int iteration = 0; iteration < maxIterations; iteration++) {
				bool hasSomethingChanged = false;

				//foreach (Tile tile in roomedMaze.Tiles) {
				for (int i = 0; i < roomedMaze.Width; i += 2) {
					for (int j = 0; j < roomedMaze.Width; j += 2) {
						Tile tile = roomedMaze[i, j];

						if (CanGrowFromTile(roomedMaze, tile)) {
							Tile withLeastWeight = GetTileWithLeastWeight(roomedMaze, tile);

							if (withLeastWeight != null) {
								roomedMaze.CutWalls(tile.Position, withLeastWeight.Position);
								hasSomethingChanged = true;
							}
						}
					}
				}

				if (!hasSomethingChanged) {
					break;
				}
			}

			return roomedMaze;
		}

		private bool CanGrowFromTile(Maze maze, Tile fromWhich) {
			if (fromWhich.Type != Tile.Variant.Empty) {
				return false;
			}

			Dictionary<Direction, bool> results = new Dictionary<Direction, bool>();
			bool result = false;

			foreach (Direction dir in Direction.Directions) {
				Coordinate whereToGo = dir.Shift(dir.Shift(fromWhich.Position)); // Double shift equals to += 2.

				if (maze.IsPointWithin(whereToGo)) {
					Tile inThatDirection = maze[whereToGo];
					results.Add(dir, inThatDirection.Type != Tile.Variant.Empty);
				}
			}

			foreach (var pair in results) {
				result = result || pair.Value;
			}

			return result;
		}

		private Tile GetTileWithLeastWeight(Maze maze, Tile fromWhich) {
			Tile withLeastWeight = null;

			foreach (Direction dir in Direction.Directions) {
				Coordinate whereToGo = dir.Shift(dir.Shift(fromWhich.Position)); // Double shift equals to += 2.

				if (maze.IsPointWithin(whereToGo)) {
					Tile inThatDirection = maze[whereToGo];

					if (inThatDirection.Type == Tile.Variant.Wall) {
						if (withLeastWeight == null) {
							withLeastWeight = inThatDirection;
						}

						if (inThatDirection.GraphWeight < withLeastWeight.GraphWeight) {
							withLeastWeight = inThatDirection;
						}
					}
				}
			}

			Debug.Assert(withLeastWeight.Type == Tile.Variant.Wall);

			return withLeastWeight;
		}
	}
}
