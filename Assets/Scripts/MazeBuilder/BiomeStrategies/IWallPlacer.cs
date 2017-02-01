using System;

public interface IWallPlacer
{
    // stateless!
    Maze PlaceWalls(Maze roomedMaze);
}


