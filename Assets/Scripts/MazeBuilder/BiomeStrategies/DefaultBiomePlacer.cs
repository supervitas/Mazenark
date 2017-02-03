using System.Collections.Generic;
using System;

public class DefaultBiomePlacer : IBiomePlacer
{
    private const int SMALL_SIZE = 32;
    private const int MEDIUM_SIZE = 64;
    private const int LARGE_SIZE = 128;

    private const int SMALL_SIZE_SAFEHOUSE = 6;
    private const int MEDIUM_SIZE_SAFEHOUSE = 10;
    private const int LARGE_SIZE_SAFEHOUSE = 18;

    private const int MIN_BIOMES = 2;
    private const int MAX_BIOMES = 5;

    private Maze maze;
    private int width;
    private int height;

    public Maze PlaceBiomes(Maze emptyMaze)
    {
        maze = emptyMaze;
        width = maze.Tiles.GetLength(0);
        height = maze.Tiles.GetLength(1);

        PlantSafehouse();
        PlantSpawns();
        PlaceBiomes();

        return emptyMaze;
    }

    private void PlantSafehouse()
    {
        int safehouseSize;
        
        if (width <= SMALL_SIZE || height <= SMALL_SIZE)
            safehouseSize = SMALL_SIZE_SAFEHOUSE;
        else if (width <= MEDIUM_SIZE || height <= MEDIUM_SIZE)
            safehouseSize = MEDIUM_SIZE_SAFEHOUSE;
        else
            safehouseSize = LARGE_SIZE_SAFEHOUSE;

        // Fill rect safehouseSize X safehouseSize exactly at the center of the maze.
        for (int i = width / 2 - safehouseSize / 2; i <= width / 2 + safehouseSize / 2; i++)
            for (int j = height / 2 - safehouseSize / 2; j <= height / 2 + safehouseSize / 2; j++)
                maze.Tiles[i, j].biome = Biome.Safehouse;

        maze.ImportantPlaces.Add(new Maze.Coordinate(width / 2, height / 2));
    }

    private void PlantSpawns()
    {
        // Should depend on size too.
        maze.Tiles[0, 0].biome = Biome.Spawn;
        maze.Tiles[0, height - 1].biome = Biome.Spawn;
        maze.Tiles[width - 1, 0].biome = Biome.Spawn;
        maze.Tiles[width - 1, height - 1].biome = Biome.Spawn;

        maze.ImportantPlaces.Add(new Maze.Coordinate(0, 0));
        maze.ImportantPlaces.Add(new Maze.Coordinate(0, height - 1));
        maze.ImportantPlaces.Add(new Maze.Coordinate(width - 1, 0));
        maze.ImportantPlaces.Add(new Maze.Coordinate(width - 1, height - 1));
    }

    private void PlaceBiomes()
    {
        Biome biome1 = Biome.getRandomBiome();   // No check if some biomes are the same.
        Biome biome2 = Biome.getRandomBiome();   
        Biome biome3 = Biome.getRandomBiome();   
        Biome biome4 = Biome.getRandomBiome();   

        for (int i = 0; i < width / 2; i++)
            for (int j = 0; j < height / 2; j++)
                if (maze.Tiles[i, j].biome == null)
                    maze.Tiles[i, j].biome = biome1;

        for (int i = 0; i < width / 2; i++)
            for (int j = height / 2; j < height; j++)
                if (maze.Tiles[i, j].biome == null)
                    maze.Tiles[i, j].biome = biome2;

        for (int i = width / 2; i < width; i++)
            for (int j = 0; j < height / 2; j++)
                if (maze.Tiles[i, j].biome == null)
                    maze.Tiles[i, j].biome = biome3;

        for (int i = width / 2; i < width; i++)
            for (int j = height / 2; j < height; j++)
                if (maze.Tiles[i, j].biome == null)
                    maze.Tiles[i, j].biome = biome4;
    }
}
