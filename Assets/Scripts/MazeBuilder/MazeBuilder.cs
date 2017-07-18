using System;
using MazeBuilder.BiomeStrategies;
using UnityEngine;
using Random = System.Random;
using MazeBuilder.Utility;
using System.Collections.Generic;

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

        public Maze Maze => _maze ?? CreateNewMaze();

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
			Random random = new Random();

			Coordinate roomRightCenter = new Coordinate(Maze.Width / 5 * 4, Maze.Height / 2);
			Coordinate roomUpCenter = new Coordinate(Maze.Width / 2, Maze.Height / 5);
			Coordinate roomLeftCenter = new Coordinate(Maze.Width / 5, Maze.Height / 2);
			Coordinate roomDownCenter = new Coordinate(Maze.Width / 2, Maze.Height / 5 * 4);
			List<Coordinate> cooordinates = new List<Coordinate>();
			cooordinates.Add(roomRightCenter);
			cooordinates.Add(roomUpCenter);
			cooordinates.Add(roomLeftCenter);
			cooordinates.Add(roomDownCenter);

			bool shouldSpawnRightRoom = random.NextDouble() < Constants.Biome.ROOM_SPAWN_CHANCE;
			bool shouldSpawnUpRoom = random.NextDouble() < Constants.Biome.ROOM_SPAWN_CHANCE;
			bool shouldSpawnLeftRoom = random.NextDouble() < Constants.Biome.ROOM_SPAWN_CHANCE;
			bool shouldSpawnDownRoom = random.NextDouble() < Constants.Biome.ROOM_SPAWN_CHANCE;
			List<bool> chances = new List<bool>();
			chances.Add(shouldSpawnRightRoom);
			chances.Add(shouldSpawnUpRoom);
			chances.Add(shouldSpawnLeftRoom);
			chances.Add(shouldSpawnDownRoom);

			int i = 0;
			foreach (Coordinate coordinate in cooordinates) {
				var tmp = coordinate;
				if (chances[i]) {
					if (tmp.X % 2 == 0)
						tmp = new Coordinate(tmp.X + 1, tmp.Y);
					if (tmp.Y % 2 == 0)
						tmp = new Coordinate(tmp.X, tmp.Y + 1);

					Room room = new Room(tmp, Constants.Biome.ROOM_MIN_SIZE, Constants.Biome.ROOM_MIN_SIZE);
					_maze.Rooms.Add(room);
					_maze.ImportantPlaces.Add(room.Center);
					_maze.CutWalls(room, _maze[tmp].Biome);
				}
				i++;
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


