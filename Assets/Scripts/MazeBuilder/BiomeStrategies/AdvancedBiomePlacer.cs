using MazeBuilder.Utility;
using System;

namespace MazeBuilder.BiomeStrategies {
    public class AdvancedBiomePlacer : IBiomePlacer {
        private const float SAFEHOUSE_FRACTION = 0.08f;
		private const int SPAWN_LENGTH = 3;

		private const int MIN_BIOMES = 3;
        private const int MAX_BIOMES = 7;

        private Maze maze;
		private Biome[,] drawLayer;
		private int tilesLeftEmpty;

		public Maze PlaceBiomes(Maze emptyMaze) {
            maze = emptyMaze;
			drawLayer = new Biome[maze.Width, maze.Height];
			tilesLeftEmpty = maze.Width * maze.Height;

            PlantSafehouse();
            PlantSpawns();
            PlantBiomes();
			GrowBiomes();

            return emptyMaze;
        }

		private void TransferDrawLayerToMaze() {
			for (int i = 0; i < maze.Width; i++) {
				for (int j = 0; j < maze.Height; j++) {
					if (drawLayer[i, j] != null) {

						ChangeMazeTileBiome(new Coordinate(i, j), drawLayer[i, j]);
						drawLayer[i, j] = null;
					}
				}
			}
		}

		private void GrowBiomesOnce() {

			foreach (Tile tile in maze.Tiles) {
				if (tile.Biome != null && !tile.Biome.IsManuallyPlaced) {
					foreach (Direction dir in Direction.Directions) {
						Coordinate neighbor = dir.Shift(tile.Position);

						if (maze.IsPointWithin(neighbor) && maze[neighbor].Biome == null) {
							drawLayer[neighbor.X, neighbor.Y] = tile.Biome;
						}
					}
				}
			}
			TransferDrawLayerToMaze();
		}

		private void GrowBiomes() {
			for (int i = 0; i < 3000; i++) {
				if (tilesLeftEmpty == 0) {
					return;
				}
				GrowBiomesOnce();
			}
		}

		private void PlantSafehouse() {
			int radius = (int) (maze.Width * SAFEHOUSE_FRACTION);

			foreach (Tile tile in maze.Tiles) {
				if (tile.Position.EuclidianDistanceTo(maze.AsRoom.Center) < radius) {
					ChangeMazeTileBiome(tile.Position, Biome.Safehouse);
				}
			}
		}

		private void PlantSpawn(Coordinate where) {
			ChangeMazeTileBiome(where, Biome.Spawn);

			for (int offset = 1; offset < SPAWN_LENGTH; offset++) {
				foreach (Direction dir in Direction.Directions) {

					Coordinate sleeve = where;
					for (int i = 0; i < offset; i++) {
						sleeve = dir.Shift(sleeve);
					}
					ChangeMazeTileBiome(sleeve, Biome.Spawn);
				}
			}
		}

        private void PlantSpawns() {
			PlantSpawn(maze.AsRoom.TopLeftCorner);
			PlantSpawn(maze.AsRoom.TopRightCorner);
			PlantSpawn(maze.AsRoom.BottomLeftCorner);
			PlantSpawn(maze.AsRoom.BottomRightCorner);
		}

		private void PlantBiomes() {
			Random random = new Random();
			int numOfBiomesLeft = random.Next(MIN_BIOMES, MAX_BIOMES + 1);

			while (numOfBiomesLeft != 0) {
				Coordinate randomPoint = new Coordinate(random.Next(0, maze.Width), random.Next(0, maze.Height));

				if (maze[randomPoint].Biome == null) {
					ChangeMazeTileBiome(randomPoint, Biome.GetRandomBiome());
					numOfBiomesLeft--;
				}
			}
		}


		// Beacuse I forgot `tilesLeftEmpty--;` several times...
		private void ChangeMazeTileBiome(Coordinate where, Biome toWhich) {
			if (maze.IsPointWithin(where) && maze[where].Biome == null) {
				maze[where].Biome = toWhich;
				if (toWhich == Biome.Spawn) {
					maze[where].Type = Tile.Variant.Empty;
				}
				tilesLeftEmpty--;
			}
		}
    }
}
