﻿using System.Collections;
using System;

public class EmptyRoomPlacer : IRoomPlacer
{
    private static EmptyRoomPlacer instance = new EmptyRoomPlacer();

    private EmptyRoomPlacer()
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
        return maze;
    }
}
