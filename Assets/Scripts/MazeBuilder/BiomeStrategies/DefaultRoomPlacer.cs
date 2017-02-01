using System.Collections;
using System;

public class DefaultRoomPlacer : IRoomPlacer
{
    private static DefaultRoomPlacer instance = new DefaultRoomPlacer();

    private DefaultRoomPlacer()
    {

    }
    static public IRoomPlacer Instance
    {
        get
        {
            return instance;
        }
    }

    public Maze PlaceRoom(Maze maze, int x, int y, int chunkLeftBoundary, int chunkRightBoundary, int chunkTopBoundary, int chunkBottomBoundary)
    {
        Random random = new Random();

        Biome targetBiome = maze.Tiles[x, y].biome;
        int width = (int) Math.Round(random.Next(Constants.Biome.ROOM_MIN_SIZE, Constants.Biome.ROOM_MAX_SIZE + 1) * targetBiome.RoomSizeModifier);
        int height = (int)Math.Round(random.Next(Constants.Biome.ROOM_MIN_SIZE, Constants.Biome.ROOM_MAX_SIZE + 1) * targetBiome.RoomSizeModifier);

        if (x + width >= chunkRightBoundary)
            x -= chunkRightBoundary - (x + width) - 1;
        if (y + height >= chunkBottomBoundary)
            y -= chunkBottomBoundary - (y + height) - 1;

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                maze.Tiles[x + i, y + j].type = Tile.Type.Empty;
                maze.Tiles[x + i, y + j].biome = targetBiome;
            }

        maze.ImportantPlaces.Add(new Maze.Coordinate(x, y));

        return maze;
    }
}
