using System;

public class MazeBuilder
{
	public int width;
	public int height;
	public IBiomePlacer biomePlacer = new DefaultBiomePlacer();

	private Maze maze;

	public MazeBuilder (int width = 10, int height = 10)
	{
		this.width = width;
		this.height = height;
	}

	public Maze Maze 
	{
		get 
		{
			return (maze != null) ? maze : CreateNewMaze();
		}
	}

	private Maze CreateNewMaze()
	{
		maze = new Maze(width, height);
		biomePlacer.PlaceBiomes(maze);
        GenerateRooms();

		return maze;
	}

	private void GenerateRooms()
	{
        int chunkSize = Constants.Maze.ROOM_CHUNK_SIZE;
        Random random = new Random();
		// split maze into 16x16 chunks and roll a dice to spawn room somewhere in it
        for (int i = 0; i * chunkSize <= width - chunkSize; i += chunkSize)   // if maze size is not a multiple of ROOM_CHUNK_SIZE, ignore things left.
            for (int j = 0; j * chunkSize <= height - chunkSize; j += chunkSize) 
            {
                int xWithinChunk = random.Next(0, chunkSize);
                int yWithinChunk = random.Next(0, chunkSize);

                int x = i * chunkSize + xWithinChunk;
                int y = j * chunkSize + yWithinChunk;

                if (y > 64)
                    System.Console.WriteLine("x: {0}, y: {1}", x , y);

                Biome biome = maze.Tiles[x, y].biome;

                float spawnChance = Constants.Biome.ROOM_SPAWN_CHANCE * biome.RoomSpawnChanceModifier;
                if (random.NextDouble() > spawnChance)
                    biome.RoomPlacer.PlaceRoom(maze, x, y, i * chunkSize, j * chunkSize, i * chunkSize + chunkSize - 1, j * chunkSize + chunkSize - 1);
            }
    }

	private void GenerateWalls()
	{
		// generate walls somehow
	}
}


