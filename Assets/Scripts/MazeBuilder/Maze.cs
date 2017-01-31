using System;

public class Maze
{
	private Tile[,] tiles;

	public Maze (int width = 10, int height = 10)
	{
        if (width < 5)
            width = 5;
        if (height < 5)
            height = 5;

        tiles = new Tile[width, height];
	}

	public Tile[,] Tiles 
	{
		get 
		{
			return tiles;
		}
	}
}


