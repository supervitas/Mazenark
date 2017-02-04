using System;

public interface IRoomPlacer{
    // stateless!
    // Boundaries are inclusive
    Maze PlaceRoom(Maze maze, int x, int y, int chunkLeftBoundary, int chunkRightBoundary, int chunkTopBoundary, int chunkBottomBoundary);
}


