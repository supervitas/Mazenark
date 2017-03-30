using System;
using MazeBuilder.Utility;
using Random = System.Random;
using System.Collections.Generic;
using System.Diagnostics;

namespace MazeBuilder.BiomeStrategies {
    public class PrimWallPlacer : IWallPlacer {
        private static PrimWallPlacer instance = new PrimWallPlacer();
        private static Random random = new Random();
		private const float CHANCE_TO_CUT_PASSAGE_THROUGH_DEAD_END = 1.0f / 2; // Every fifth will be cut through.

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
				bool attemptToPlacePassage = false;	// More the random is better.

				//foreach (Tile tile in roomedMaze.Tiles) {
				for (int i = 0; i < roomedMaze.Width; i += 2) {
					for (int j = 0; j < roomedMaze.Width; j += 2) {
						Tile tile = roomedMaze[i, j];

						attemptToPlacePassage = PlaceWallIfRandomSaysSo();

						if (attemptToPlacePassage && CanGrowFromTile(roomedMaze, tile)) {
							Tile withLeastWeight = GetTileWithLeastWeight(roomedMaze, tile);

							if (withLeastWeight != null) {
								roomedMaze.CutPassage(tile.Position, withLeastWeight.Position);
								hasSomethingChanged = true;
							}
						}
					}
				}

				if (!hasSomethingChanged && attemptToPlacePassage) {
					break;
				}
			}

			CutThroughDeadEnds(roomedMaze);
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
					// Growth can be only towards Wall tiles.
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

					if (inThatDirection.Type != Tile.Variant.Empty) {
						if (withLeastWeight == null) {
							withLeastWeight = inThatDirection;
						}

						if (inThatDirection.GraphWeight < withLeastWeight.GraphWeight) {
							withLeastWeight = inThatDirection;
						}
					}
				}
			}

			return withLeastWeight;
		}

		private bool PlaceWallIfRandomSaysSo() {
			return random.Next(101) > 75;  // 25% chance
		}

		private void CutThroughDeadEnds(Maze maze) {
			for (int i = 0; i < maze.Width; i += 2) {
				for (int j = 0; j < maze.Width; j += 2) {
					Tile tile = maze[i, j];

					var deadEndDirection = IsDeadEnd(maze, tile);

					if (deadEndDirection != null) {
						Coordinate blockingWallPosition = deadEndDirection.Shift(tile.Position);

						if (maze.IsPointWithin(blockingWallPosition)) {
							bool shouldCutThrough = random.NextDouble() < CHANCE_TO_CUT_PASSAGE_THROUGH_DEAD_END;

							if (shouldCutThrough) {
								maze[blockingWallPosition].Type = Tile.Variant.Empty;
							}
						}
					}
				}
			}
		}

		private Direction IsDeadEnd(Maze maze, Tile fromWhich) {
			Direction entrancePosition = null;
			int wallsCounter = 0;

			foreach (Direction dir in Direction.Directions) {
				Coordinate whereToGo = dir.Shift(dir.Shift(fromWhich.Position)); // Double shift equals to += 2.

				if (maze.IsPointWithin(whereToGo)) {
					Tile inThatDirection = maze[whereToGo];
					if (inThatDirection.Type == Tile.Variant.Wall) {
						wallsCounter++;
					}
					if (inThatDirection.Type == Tile.Variant.Empty) {
						entrancePosition = dir;
					}
				}
			}

			if (entrancePosition != null)
				return entrancePosition.Opposite;
			else
				return null;

			// ↓ This won't work...
			// return entrancePosition.Opposite;
		}
	}
}
