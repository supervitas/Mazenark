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
			return (maze != null) ? maze : createNewMaze();
		}
	}

	private Maze createNewMaze()
	{
		maze = new Maze(width, height);
		biomePlacer.placeBiomes(maze);

		return maze;
	}

	private void generateRooms()
	{
		// split maze into 16x16 chunks and roll a dice to spawn room somewhere in it
	}

	private void generateWalls()
	{
		// generate walls somehow
	}
}


