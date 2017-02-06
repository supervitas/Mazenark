using System;
using UnityEngine.PlaymodeTests;

public class MazeBuilder {
	public int width;
	public int height;
	public IBiomePlacer biomePlacer = new DefaultBiomePlacer();

	private Maze maze;

	public MazeBuilder (int width = 10, int height = 10) {
		this.width = width;
		this.height = height;
	}

	public Maze Maze {
		get {
			return maze ?? CreateNewMaze();
		}
	}

	private Maze CreateNewMaze() {
		maze = new Maze(width, height);
		biomePlacer.PlaceBiomes(maze);
        GenerateRooms();
        GenerateWalls();


		return maze;
	}

	private void GenerateRooms() {
        var chunkSize = Constants.Maze.ROOM_CHUNK_SIZE;
        var random = new Random();
		// split maze into 16x16 chunks and roll a dice to spawn room somewhere in it
        for (var i = 0; i <= width - chunkSize; i += chunkSize)   // if maze size is not a multiple of ROOM_CHUNK_SIZE, ignore things left.
            for (var j = 0; j <= height - chunkSize; j += chunkSize) {
                var xWithinChunk = random.Next(0, chunkSize);
                var yWithinChunk = random.Next(0, chunkSize);

                var x = i + xWithinChunk;
                var y = j + yWithinChunk;

                if (x > 64 || y > 64)
                    System.Console.WriteLine("x: {0}, y: {1}", x, y);

                var biome = maze.Tiles[x, y].Biome;

                var spawnChance = Constants.Biome.ROOM_SPAWN_CHANCE * biome.RoomSpawnChanceModifier;
                if (random.NextDouble() < spawnChance)
                    biome.RoomPlacer.PlaceRoom(x: x, y: y, chunkLeftBoundary: i, chunkRightBoundary: j,
                        chunkTopBoundary: i + chunkSize - 1, chunkBottomBoundary: j + chunkSize - 1, maze: maze);
            }
    }

	private void GenerateWalls() {
        // It should be per-biome strategy, not global!
        DefaultWallPlacer.Instance.PlaceWalls(maze);
	}

    private void MakeSpawnPoints() {
        //Todo
    }
}


