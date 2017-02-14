using System.Collections;
using System;

public class DefaultWallPlacer : IWallPlacer{
    private static DefaultWallPlacer instance = new DefaultWallPlacer();

    private DefaultWallPlacer()
    {

    }
    public static IWallPlacer Instance{
        get{
            return instance;
        }
    }


    // Fully connected pois.
    public Maze PlaceWalls(Maze maze){
        var random = new Random();

        // poi == place of interest.
        foreach (var poiFrom in maze.ImportantPlaces) {
            foreach (var poiTo in maze.ImportantPlaces) {
                var shouldCorridorGoUpFirst = random.Next(2) == 0;
                var xFrom = poiFrom.X;
                var yFrom = poiFrom.Y;
                var xTo = poiTo.X;
                var yTo = poiTo.Y;

                if (shouldCorridorGoUpFirst) {
                    makeVerticalCorridor(maze, yFrom, yTo, xFrom);
                    makeHorizontalCorridor(maze, xFrom, xTo, yTo);
                } else {
                    makeHorizontalCorridor(maze, xFrom, xTo, yFrom);
                    makeVerticalCorridor(maze, yFrom, yTo, xTo);
                }
            }
        }

        return maze;
    }

    private void makeVerticalCorridor(Maze maze, int yFrom, int yTo, int x) {
        var lesserY = yFrom < yTo ? yFrom: yTo;
        var greaterY = yFrom > yTo ? yFrom : yTo;
        for (var j = lesserY; j <= greaterY; j++)
            maze.Tiles[x, j].type = Tile.Type.Empty;
    }

    private void makeHorizontalCorridor(Maze maze, int xFrom, int xTo, int y) {
        var lesserX = (xFrom < xTo) ? xFrom : xTo;
        var greaterX = (xFrom > xTo) ? xFrom : xTo;
        for (var i = lesserX; i <= greaterX; i++)
            maze.Tiles[i, y].type = Tile.Type.Empty;
    }
}
