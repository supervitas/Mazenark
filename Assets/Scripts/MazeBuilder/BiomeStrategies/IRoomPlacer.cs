using System;

public interface IRoomPlacer
{
	Maze PlaceRoom(int x, int y, int chunkLeftBoundary, int chunkRightBoundary, int chunkTopBoundary, int chunkBottomBoundary);
}


