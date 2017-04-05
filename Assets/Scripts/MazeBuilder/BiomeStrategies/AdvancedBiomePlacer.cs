using MazeBuilder.Utility;
using System;

namespace MazeBuilder.BiomeStrategies {
    public class AdvancedBiomePlacer : IBiomePlacer {
        private const float SAFEHOUSE_FRACTION = 0.16f;
		private const int SPAWN_LENGTH = 3;

		private const int MIN_BIOMES = 4;
        private const int MAX_BIOMES = 8;

        private Maze maze;
		private Biome[,] drawLayer;
		private int[,] biomeIDLayer;
		private int tilesLeftEmpty;

		private int biomeIDCounter = 0;

		public Maze PlaceBiomes(Maze emptyMaze) {
            maze = emptyMaze;
			drawLayer = new Biome[maze.Width, maze.Height];
			biomeIDLayer = new int[maze.Width, maze.Height];
			tilesLeftEmpty = maze.Width * maze.Height;

            PlantSafehouse();
            PlantSpawns();
            PlantBiomes();
			GrowBiomes();

			WriteBiomesListIntoMaze(biomeIDCounter, maze);

            return emptyMaze;
        }

		private void TransferDrawLayerToMaze() {
			for (int i = 0; i < maze.Width; i++) {
				for (int j = 0; j < maze.Height; j++) {
				    if (drawLayer[i, j] == null) continue;
				    ChangeMazeTileBiome(new Coordinate(i, j), drawLayer[i, j], biomeIDLayer[i, j]);
				    drawLayer[i, j] = null;
				}
			}
		}

		private bool ShouldGrowBiome(int iteration, float growthDeceleration) {
			float howMuchItHasGrownAlready = iteration * growthDeceleration;
			float howMuchItHasGrownOnPreviousIteration = (iteration - 1) * growthDeceleration;
			// E.g. floor(3.75) - floor(3.5) will yield 0, and floor(4.01) - floor(3.75) will yield 1.
			bool wasTransistionOverPoint = Math.Floor(howMuchItHasGrownAlready) - Math.Floor(howMuchItHasGrownOnPreviousIteration) > 1.0f - 10e-6;  // 1.0f - 10e-6 looks like 0.9, but closer to 1.0f

			return wasTransistionOverPoint;
		}

		private void GrowBiomesOnce(int iteration) {

			foreach (Tile tile in maze.Tiles) {
				if (tile.Biome != null && !tile.Biome.IsManuallyPlaced) {
					bool shouldGrow = ShouldGrowBiome(iteration, tile.Biome.SizeModifier);

					if (shouldGrow) {
						foreach (Direction dir in Direction.Directions) {
							Coordinate neighbor = dir.Shift(tile.Position);

							if (maze.IsPointWithin(neighbor) && maze[neighbor].Biome == null) {
								drawLayer[neighbor.X, neighbor.Y] = tile.Biome;
								biomeIDLayer[neighbor.X, neighbor.Y] = tile.BiomeID;
							}
						}
					}
				}
			}
			TransferDrawLayerToMaze();
		}

		private void GrowBiomes() {
			for (int i = 1; i < 3000; i++) {
				if (tilesLeftEmpty == 0) {
					return;
				}
				GrowBiomesOnce(i);
			}
		}

		private void PlantSafehouse() {
			int radius = (int) (maze.Width * SAFEHOUSE_FRACTION / 2);

			int minX = int.MaxValue;
			int minY = int.MaxValue;
			int maxX = int.MinValue;
			int maxY = int.MinValue;

			foreach (Tile tile in maze.Tiles) {
				if (tile.Position.EuclidianDistanceTo(maze.AsRoom.Center) < radius) {
					ChangeMazeTileBiome(tile.Position, Biome.Safehouse, biomeIDCounter);

					int x = tile.Position.X;
					int y = tile.Position.Y;
					if (x < minX) minX = x;
					if (x > maxX) maxX = x;
					if (y < minY) minY = y;
					if (y > maxY) maxY = y;
					
				}
			}

			maze.Rooms.Add(new Room(minX, minY, maxX, maxY));
		}

		private void PlantSpawn(Coordinate where) {
			ChangeMazeTileBiome(where, Biome.Spawn, biomeIDCounter);

			for (int offset = 1; offset < SPAWN_LENGTH; offset++) {
				foreach (Direction dir in Direction.Directions) {

					Coordinate sleeve = where;
					for (int i = 0; i < offset; i++) {
						sleeve = dir.Shift(sleeve);
					}
					ChangeMazeTileBiome(sleeve, Biome.Spawn, biomeIDCounter);
				}
			}
		}

        private void PlantSpawns() {
			biomeIDCounter++;
			PlantSpawn(maze.AsRoom.TopLeftCorner);
			biomeIDCounter++;
			PlantSpawn(maze.AsRoom.TopRightCorner);
			biomeIDCounter++;
			PlantSpawn(maze.AsRoom.BottomLeftCorner);
			biomeIDCounter++;
			PlantSpawn(maze.AsRoom.BottomRightCorner);
		}


		private void PlantBiomes() {
			Random random = new Random();
			int numOfBiomes = random.Next(MIN_BIOMES, MAX_BIOMES + 1);

			int minimalDistanceFromCenter = (int) (maze.Width * SAFEHOUSE_FRACTION / 2) + 1;
			int maximalDistanceFromCenter = (int) (maze.Width * 0.8f / 2);
			double radiansBetweenSpawns = 2 * Math.PI / numOfBiomes;

			for (int i = 0; i < numOfBiomes; i++) {
				int randomDistance = random.Next(minimalDistanceFromCenter, maximalDistanceFromCenter + 1);
				int offsetX = (int) Math.Round(randomDistance * Math.Cos(radiansBetweenSpawns * i));
				int offsetY = (int) Math.Round(randomDistance * Math.Sin(radiansBetweenSpawns * i));

				Coordinate randomPoint = new Coordinate(offsetX + maze.Width / 2, offsetY + maze.Height / 2);

				ChangeMazeTileBiome(randomPoint, Biome.GetRandomBiome(), biomeIDCounter);
				maze[randomPoint].BiomeID = biomeIDCounter++;
			}
		}


		// Beacuse I forgot `tilesLeftEmpty--;` several times...
		private void ChangeMazeTileBiome(Coordinate where, Biome toWhich, int biomeID) {
			if (maze.IsPointWithin(where) && maze[where].Biome == null) {
				maze[where].Biome = toWhich;
				maze[where].BiomeID = biomeID;
				if (toWhich == Biome.Spawn || toWhich == Biome.Safehouse) {
					maze[where].Type = Tile.Variant.Empty;
				}
				tilesLeftEmpty--;
			}
		}

		public static void WriteBiomesListIntoMaze(int maxBiomeID, Maze maze) {
			for (int i = 0; i < maxBiomeID; i++) {
				Maze.TileCollection biome = null;

				foreach (Tile tile in maze.Tiles) {
					if (tile.BiomeID == i) {
						if (biome == null) {
							biome = new Maze.TileCollection(tile.Biome);
							maze.Biomes.Add(biome);
						}
						biome.tiles.Add(tile);
					}
				}
			}
		}
	}
}
