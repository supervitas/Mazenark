using System;
using MazeBuilder.BiomeStrategies;

namespace MazeBuilder {
    public class MazeBuilder {
        public int Width;
        public int Height;
        public IBiomePlacer BiomePlacer = new DefaultBiomePlacer();

        private Maze maze;

        public MazeBuilder (int width = 10, int height = 10) {
            Width = width;
            Height = height;
        }

        public Maze Maze {
            get {
                return maze ?? CreateNewMaze();
            }
        }

        private Maze CreateNewMaze() {
            maze = new Maze(Width, Height);
            BiomePlacer.PlaceBiomes(maze);
            GenerateRooms();
			PlaceTileWeights();
            GenerateWalls();

            return maze;
        }

        private void GenerateRooms() {
            var chunkSize = Constants.Maze.ROOM_CHUNK_SIZE;
            var random = new Random();
            // split maze into 16x16 chunks and roll a dice to spawn room somewhere in it
            for (var i = 0; i <= Width - chunkSize; i += chunkSize)   // if maze size is not a multiple of ROOM_CHUNK_SIZE, ignore things left.
            for (var j = 0; j <= Height - chunkSize; j += chunkSize) {
                var xWithinChunk = random.Next(0, chunkSize);
                var yWithinChunk = random.Next(0, chunkSize);

                var x = i + xWithinChunk;
                var y = j + yWithinChunk;

                if (x > 64 || y > 64)
                    Console.WriteLine("x: {0}, y: {1}", x, y);

                var biome = maze.Tiles[x, y].Biome;

                var spawnChance = Constants.Biome.ROOM_SPAWN_CHANCE * biome.RoomSpawnChanceModifier;
                if (random.NextDouble() < spawnChance)
                    biome.RoomPlacer.PlaceRoom(x: x, y: y, chunkLeftBoundary: i, chunkRightBoundary: i + chunkSize - 1,
                        chunkTopBoundary: j, chunkBottomBoundary: j + chunkSize - 1, maze: maze);
            }
        }

		private void PlaceTileWeights() {
			foreach (Tile tile in maze.Tiles) {
				tile.Biome.TileWeighter.SetTileWeight(maze, tile.Position);
			}
		}


		private void GenerateWalls() {
			// It should be per-biome strategy, not global!
			PrimWallPlacer.Instance.PlaceWalls(maze);
        }

        private void MakeSpawnPoints() {
            //Todo
        }
    }
}


