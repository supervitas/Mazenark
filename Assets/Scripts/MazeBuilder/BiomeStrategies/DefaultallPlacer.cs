using System.Collections;
using System;

public class DefaultWallPlacer : IWallPlacer
{
    private static DefaultWallPlacer instance = new DefaultWallPlacer();

    private DefaultWallPlacer()
    {

    }
    static public IWallPlacer Instance
    {
        get
        {
            return instance;
        }
    }


    // Fully connected pois.
    public Maze PlaceWalls(Maze maze)
    {
        Random random = new Random();

        // poi == place of interest.
        foreach (Maze.Coordinate poiFrom in maze.ImportantPlaces) {
            foreach (Maze.Coordinate poiTo in maze.ImportantPlaces)
            {
                bool shouldCorridorGoUpFirst = random.Next(2) == 0;
                int xFrom = poiFrom.X;
                int yFrom = poiFrom.Y;
                int xTo = poiTo.X;
                int yTo = poiTo.Y;

                if (shouldCorridorGoUpFirst)
                {
                    makeVerticalCorridor(maze, yFrom, yTo, xFrom);
                    makeHorizontalCorridor(maze, xFrom, xTo, yTo);
                } else
                {
                    makeHorizontalCorridor(maze, xFrom, xTo, yFrom);
                    makeVerticalCorridor(maze, yFrom, yTo, xTo);
                }
            }
        }

        return maze;
    }

    private void makeVerticalCorridor(Maze maze, int yFrom, int yTo, int x)
    {
        int lesserY = (yFrom < yTo) ? yFrom: yTo;
        int greaterY = (yFrom > yTo) ? yFrom : yTo;
        for (int j = lesserY; j <= greaterY; j++)
            maze.Tiles[x, j].type = Tile.Type.Empty;
    }

    private void makeHorizontalCorridor(Maze maze, int xFrom, int xTo, int y)
    {
        int lesserX = (xFrom < xTo) ? xFrom : xTo;
        int greaterX = (xFrom > xTo) ? xFrom : xTo;
        for (int i = lesserX; i <= greaterX; i++)
            maze.Tiles[i, y].type = Tile.Type.Empty;
    }
}
