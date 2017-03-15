using MazeBuilder.Utility;
using System;

namespace MazeBuilder.BiomeStrategies {
    public class AdvancedBiomePlacer : IBiomePlacer {
        private const float SAFEHOUSE_FRACTION = 0.08f;
		private const int SPAWN_LENGTH = 3;

		private const int MIN_BIOMES = 6;
        private const int MAX_BIOMES = 12;

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

			WriteBiomesListIntoMaze();

            return emptyMaze;
        }

		private void TransferDrawLayerToMaze() {
			for (int i = 0; i < maze.Width; i++) {
				for (int j = 0; j < maze.Height; j++) {
					if (drawLayer[i, j] != null) {

						ChangeMazeTileBiome(new Coordinate(i, j), drawLayer[i, j], biomeIDLayer[i, j]);
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
							biomeIDLayer[neighbor.X, neighbor.Y] = tile.BiomeID;
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
					ChangeMazeTileBiome(tile.Position, Biome.Safehouse, biomeIDCounter);
				}
			}
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
			int numOfBiomesLeft = random.Next(MIN_BIOMES, MAX_BIOMES + 1);

			while (numOfBiomesLeft != 0) {
				Coordinate randomPoint = new Coordinate(random.Next(0, maze.Width), random.Next(0, maze.Height));

				if (maze[randomPoint].Biome == null) {
					biomeIDCounter++;
					ChangeMazeTileBiome(randomPoint, Biome.GetRandomBiome(), biomeIDCounter);
					numOfBiomesLeft--;
				}
			}
		}


		// Beacuse I forgot `tilesLeftEmpty--;` several times...
		private void ChangeMazeTileBiome(Coordinate where, Biome toWhich, int biomeID) {
			if (maze.IsPointWithin(where) && maze[where].Biome == null) {
				maze[where].Biome = toWhich;
				maze[where].BiomeID = biomeID;
				if (toWhich == Biome.Spawn) {
					maze[where].Type = Tile.Variant.Empty;
				}
				tilesLeftEmpty--;
			}
		}

		private void WriteBiomesListIntoMaze() {
			for (int i = 0; i < biomeIDCounter; i++) {
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
