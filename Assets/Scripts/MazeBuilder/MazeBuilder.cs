using System;
using MazeBuilder.BiomeStrategies;
using UnityEngine;
using Random = System.Random;

namespace MazeBuilder {
    public class MazeBuilder {
        public int Width;
        public int Height;
        public IBiomePlacer BiomePlacer = new AdvancedBiomePlacer();

        private Maze _maze;

        public MazeBuilder (int width = 10, int height = 10, Maze maze = null) {
            Width = width;
            Height = height;
            if (maze != null) {
                _maze = maze;
            }
        }

        public Maze Maze {
            get {
                return _maze ?? CreateNewMaze();
            }

        }

        private Maze CreateNewMaze() {
            _maze = new Maze(Width, Height);
            BiomePlacer.PlaceBiomes(_maze);
            GenerateRooms();
			PlaceTileWeights();
            GenerateWalls();
			EncloseMazeWithWalls();

			_maze.GenerateBiomesList();

			return _maze;
        }

        private void GenerateRooms() {
            var chunkSize = global::Constants.Maze.ROOM_CHUNK_SIZE;
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

                var biome = _maze.Tiles[x, y].Biome;

                var spawnChance = global::Constants.Biome.ROOM_SPAWN_CHANCE * biome.RoomSpawnChanceModifier;
                if (random.NextDouble() < spawnChance)
                    biome.RoomPlacer.PlaceRoom(x: x, y: y, chunkLeftBoundary: i, chunkRightBoundary: i + chunkSize - 1,
                        chunkTopBoundary: j, chunkBottomBoundary: j + chunkSize - 1, maze: _maze);

            }
        }

		private void PlaceTileWeights() {
			foreach (Tile tile in _maze.Tiles) {
				tile.Biome.TileWeighter.SetTileWeight(_maze, tile.Position);
			}
		}


		private void GenerateWalls() {
			// It should be per-biome strategy, not global!
			PrimWallPlacer.Instance.PlaceWalls(_maze);
        }

		private void EncloseMazeWithWalls() {
			for (int i = 0; i < Maze.Width; i++) {
				var tileUp = Maze[i, 0];
				var tileDown = Maze[i, Maze.Height - 1];

				if (tileUp.Biome != Biome.Spawn)
					tileUp.Type = Tile.Variant.Wall;
				if (tileDown.Biome != Biome.Spawn)
					tileDown.Type = Tile.Variant.Wall;
			}
			for (int j = 0; j < Maze.Height; j++) {
				var tileLeft = Maze[0, j];
				var tileRight = Maze[Maze.Width - 1, j];

				if (tileLeft.Biome != Biome.Spawn)
					tileLeft.Type = Tile.Variant.Wall;
				if (tileRight.Biome != Biome.Spawn)
					tileRight.Type = Tile.Variant.Wall;
			}
		}

    }
}


