using System;

public class Maze
{
	private Tile[,] tiles;

	public Maze (int width = 10, int height = 10)
	{
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


