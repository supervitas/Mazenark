using System;
using System.Collections.Generic;
using MazeBuilder.Utility;

namespace MazeBuilder.BiomeStrategies {
    public class DefaultWallPlacer : IWallPlacer{
        private static DefaultWallPlacer instance = new DefaultWallPlacer();
        private static Random random = new Random();
        // For 1 way, 2 ways, 3 ways and 4 ways accordingly.
        private static double[] chanceOfWays = { 1.0, 0.4, 0.1, 0.03 };

        private DefaultWallPlacer() { }
        public static IWallPlacer Instance {
            get {
                return instance;
            }
        }


        // Fully connected pois.
        public Maze PlaceWalls(Maze maze) {
            for (var i = 0; i < maze.Width; i += 2)
            for (var j = 0; j < maze.Height; j += 2)
                MakeRandomPassagesAt(maze, i, j);

            return maze;
        }

        private void MakeRandomPassagesAt(Maze maze, int x, int y) {
            maze.Tiles[x, y].Type = Tile.Variant.Empty;
            var numOfWays = random.NextDouble() < chanceOfWays[0] ? 1 : 0;
            numOfWays = random.NextDouble() < chanceOfWays[1] ? 2 : numOfWays;
            numOfWays = random.NextDouble() < chanceOfWays[2] ? 3 : numOfWays;
            numOfWays = random.NextDouble() < chanceOfWays[3] ? 4 : numOfWays;

            var directions = new HashSet<Direction>();
            for (; directions.Count != numOfWays; ) {
                var direction = Direction.Directions[random.Next(0, 4)];
                directions.Add(direction);
            }

            foreach (Direction direction in directions)
                if (maze.IsPointWithin(direction.ApplyToPoint(x, y).X, direction.ApplyToPoint(x, y).Y))
                    maze.Tiles[direction.ApplyToPoint(x, y).X, direction.ApplyToPoint(x, y).Y].Type = Tile.Variant.Empty;
        }

        private class Direction {
            public static List<Direction> Directions = new List<Direction>();
            public int DeltaX {
                get; private set;
            }
            public int DeltaY {
                get; private set;
            }
            public double ChanceToSpawn {
                get; private set;
            }
            private Direction(int dx, int dy, double chance) {
                DeltaX = dx;
                DeltaY = dy;
                ChanceToSpawn = chance;
                Directions.Add(this);
            }

            public Coordinate ApplyToPoint(int x, int y) {
                return new Coordinate(x + DeltaX, y + DeltaY);
            }

            public Coordinate ApplyToPoint(Coordinate point) {
                return new Coordinate(point.X + DeltaX, point.Y + DeltaY);
            }

            public static Direction Up    = new Direction(0, -1, 0.15);
            public static Direction Left  = new Direction(-1, 0, 0.01);	// Never go left!
            public static Direction Down  = new Direction(0, +1, 0.1);
            public static Direction Right = new Direction(+1, 0, 0.4);
        }
    }
}
