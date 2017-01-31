using System;

public class MazeBuilder
{
	public int width;
	public int height;
	public IBiomePlacer biomePlacer;

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

		return maze;
	}

	private void GenerateRooms()
	{
		// split maze into 16x16 chunks and roll a dice to spawn room somewhere in it
	}

	private void GenerateWalls()
	{
		// generate walls somehow
	}
}


