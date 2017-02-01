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
    private int unaffectedTiles;
    private List<BiomeSeed> startingPoints = new List<BiomeSeed>();

    public Maze PlaceBiomes(Maze emptyMaze)
    {
        maze = emptyMaze;
        width = maze.Tiles.GetLength(0);
        height = maze.Tiles.GetLength(1);
        unaffectedTiles = width * height;

        PlantSafehouse();
        PlantSpawns();
        PlantBiomeSeeds();
        GrowBiomeSeeds();

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

        unaffectedTiles -= safehouseSize * safehouseSize;
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

        unaffectedTiles -= 4;
    }

    private void PlantBiomeSeeds()
    {
        Random random = new Random();
        int numOfBiomes = random.Next(MIN_BIOMES, MAX_BIOMES + 1);
        int degreesPerBiome = 360 / numOfBiomes;
        int firstBiomeOffset = random.Next(0, 360);
        int smallestDistance = (width < height) ? width / 2 : height / 2;

        for (int i = 0; i < numOfBiomes; i++)
        {
            Biome biome = Biome.getRandomBiome();   // No check if some biomes are the same.
            int randomDistance = random.Next(smallestDistance);

            int x = smallestDistance + GetRotatedX(randomDistance, (firstBiomeOffset + degreesPerBiome) % 360);
            int y = smallestDistance + GetRotatedY(randomDistance, (firstBiomeOffset + degreesPerBiome) % 360);

            startingPoints.Add(new BiomeSeed(x, y, biome));
        }
    }

    private void GrowBiomeSeeds()
    {
        while (unaffectedTiles > 0)
        {
            int iteration = 0;  // Manhattan-distanced radius at which check tiles.
            foreach (BiomeSeed point in startingPoints)
            {
                int x = point.x;
                int y = point.y;

                // Very bad quality of code.
                for (int i = x - iteration; i <= x + iteration; i++)
                    for (int j = y - iteration; j <= y + iteration; j++)
                        if (i > 0 && i < width && j > 0 && j < height)  // Check if we are still within boundaries
                            if (maze.Tiles[i, j].biome != null)  // Here comes O(n^2)
                            {
                                int dx = Math.Abs(x - i);
                                int dy = Math.Abs(y - j);
                                if (dx + dy <= iteration)
                                {
                                    maze.Tiles[i, j].biome = point.biome;
                                    unaffectedTiles--;
                                }
                            }
            }
            iteration++;
                    
        }
    }

    private int GetRotatedX(int distance, int angleDegrees)
    {
        double angle = Math.PI * angleDegrees / 180;
        return (int) Math.Round(Math.Cos(angle) * distance) - 1;    // -1 is for safety. Otherwise there may be out-of-bounds access to array.

    }

    private int GetRotatedY(int distance, int angleDegrees)
    {
        double angle = Math.PI * angleDegrees / 180;
        return (int)Math.Round(Math.Sin(angle) * distance) - 1;    // -1 is for safety. Otherwise there may be out-of-bounds access to array.
    }

    private class BiomeSeed
    {
        public int x;
        public int y;
        public Biome biome;

        public BiomeSeed(int x, int y, Biome type)
        {
            this.x = x;
            this.y = y;
            this.biome = type;
        }
    }

}
