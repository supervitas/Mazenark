using MazeBuilder.Utility;

namespace MazeBuilder.BiomeStrategies {
    public class DefaultBiomePlacer : IBiomePlacer {
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

        public Maze PlaceBiomes(Maze emptyMaze) {
            maze = emptyMaze;
            width = maze.Tiles.GetLength(0);
            height = maze.Tiles.GetLength(1);

            PlantSafehouse();
            PlantSpawns();
            PlaceBiomes();

            return emptyMaze;
        }

        private void PlantSafehouse() {
            int safehouseSize;

            if (width <= SMALL_SIZE || height <= SMALL_SIZE)
                safehouseSize = SMALL_SIZE_SAFEHOUSE;
            else if (width <= MEDIUM_SIZE || height <= MEDIUM_SIZE)
                safehouseSize = MEDIUM_SIZE_SAFEHOUSE;
            else
                safehouseSize = LARGE_SIZE_SAFEHOUSE;

            int leftX = width / 2 - safehouseSize / 2;
            int rightX = width / 2 + safehouseSize / 2;
            int topY = height / 2 - safehouseSize / 2;
            int bottomY = height / 2 + safehouseSize / 2;

            var room = new Room(new Coordinate(leftX, topY), new Coordinate(rightX, bottomY));
            maze.CutWalls(room, Biome.Safehouse);
            maze.Rooms.Add(room);
            maze.ImportantPlaces.Add(room.Center);
        }

        private void PlantSpawns() {
            // Should depend on size too.
            maze.Tiles[0, 0].Biome = Biome.Spawn;
			maze.Tiles[0, 0].Type = Tile.Variant.Empty;
			maze.Tiles[0, height - 1].Biome = Biome.Spawn;
			maze.Tiles[0, height - 1].Type = Tile.Variant.Empty;
			maze.Tiles[width - 1, 0].Biome = Biome.Spawn;
			maze.Tiles[width - 1, 0].Type = Tile.Variant.Empty;
			maze.Tiles[width - 1, height - 1].Biome = Biome.Spawn;
			maze.Tiles[width - 1, height - 1].Type = Tile.Variant.Empty;

			maze.ImportantPlaces.Add(new Coordinate(0, 0));
            maze.ImportantPlaces.Add(new Coordinate(0, height - 1));
            maze.ImportantPlaces.Add(new Coordinate(width - 1, 0));
            maze.ImportantPlaces.Add(new Coordinate(width - 1, height - 1));
        }

        private void PlaceBiomes() {
            var biome1 = Biome.GetRandomBiome();   // No check if some biomes are the same.
            var biome2 = Biome.GetRandomBiome();
            var biome3 = Biome.GetRandomBiome();
            var biome4 = Biome.GetRandomBiome();

            for (var i = 0; i < width / 2; i++)
            for (var j = 0; j < height / 2; j++)
                if (maze.Tiles[i, j].Biome == null)
                    maze.Tiles[i, j].Biome = biome1;

            for (var i = 0; i < width / 2; i++)
            for (var j = height / 2; j < height; j++)
                if (maze.Tiles[i, j].Biome == null)
                    maze.Tiles[i, j].Biome = biome2;

            for (var i = width / 2; i < width; i++)
            for (var j = 0; j < height / 2; j++)
                if (maze.Tiles[i, j].Biome == null)
                    maze.Tiles[i, j].Biome = biome3;

            for (var i = width / 2; i < width; i++)
            for (var j = height / 2; j < height; j++)
                if (maze.Tiles[i, j].Biome == null)
                    maze.Tiles[i, j].Biome = biome4;
        }
    }
}
