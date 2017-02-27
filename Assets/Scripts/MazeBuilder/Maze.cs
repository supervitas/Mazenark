using System.Collections.Generic;

namespace MazeBuilder {
    public class Maze {
        // First index is x-coordinate, second index is y-coordinate.
        // [0, 0] stands for topmost leftmost square.
        // 0----5----10---→ x
        // |
        // |
        // ↓
        // y
        private Tile[,] tiles;
        private List<Coordinate> importantPlaces = new List<Coordinate>();  // Should have at least one path leading to them.
        private List<Room> rooms = new List<Room>();

        public Maze (int width = 10, int height = 10) {
            if (width < 5)
                width = 5;
            if (height < 5)
                height = 5;

            tiles = new Tile[width, height];

            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                tiles[i, j] = new Tile();

        }

        public Tile[,] Tiles {
            get {
                return tiles;
            }
        }

        public List<Coordinate> ImportantPlaces{
            get {
                return importantPlaces;
            }
        }

        public List<Room> Rooms {
            get {
                return rooms;
            }
        }

        public int Width {
            get { return tiles.GetLength(0); }
        }

        public int Height {
            get { return tiles.GetLength(1); }
        }

        public void CutWalls(Coordinate topLeft, Coordinate bottomRight) {
            CutWalls(new Room(topLeft, bottomRight));
        }

        public void CutWalls(Room room, Biome fillWith = null) {
            for (var i = room.TopLeftCorner.X; i <= room.TopRightCorner.X; i++)
            for (var j = room.TopLeftCorner.Y; j <= room.BottomLeftCorner.Y; j++) {
                tiles[i, j].type = Tile.Type.Empty;
                if (fillWith != null)
                    tiles[i, j].biome = fillWith;
            }
        }

        public class Coordinate {
            public Coordinate(int x, int y) {
                X = x;
                Y = y;
            }

            public int X { get; private set; }

            public int Y { get; private set; }
        }

        public class Room {
            private Coordinate topLeft;
            private Coordinate bottomRight;

            public Room(Coordinate topLeft, Coordinate bottomRight) {
                this.topLeft = topLeft;
                this.bottomRight = bottomRight;
            }

            public Room(int topLeftXCoordinate, int topLeftYCoordinate, int bottomRightXCoordinate, int bottomRightYCoordinate) {
                int lesserX = topLeftXCoordinate < bottomRightXCoordinate ? topLeftXCoordinate : bottomRightXCoordinate;
                int greaterX = topLeftXCoordinate < bottomRightXCoordinate ? bottomRightXCoordinate : topLeftXCoordinate;
                int lesserY = topLeftYCoordinate < bottomRightYCoordinate ? topLeftYCoordinate : bottomRightYCoordinate;
                int greaterY = topLeftYCoordinate < bottomRightYCoordinate ? bottomRightYCoordinate : topLeftYCoordinate;

                topLeft = new Coordinate(lesserX, lesserY);
                bottomRight = new Coordinate(greaterX, greaterY);
            }

            public Coordinate TopLeftCorner {
                get {
                    return topLeft;
                }
            }

            public Coordinate TopRightCorner {
                get {
                    return new Coordinate(bottomRight.X, topLeft.Y);
                }
            }

            public Coordinate BottomRightCorner {
                get {
                    return bottomRight;
                }
            }

            public Coordinate BottomLeftCorner {
                get {
                    return new Coordinate(topLeft.X, bottomRight.Y);
                }
            }

            // Will return top-left coordinate of four center squares if room has even sides.
            public Coordinate Center {
                get {
                    return new Coordinate((topLeft.X + bottomRight.X) / 2, (topLeft.Y + bottomRight.Y) / 2);
                }
            }

            public int Width {
                get {
                    return bottomRight.X - topLeft.X;
                }
            }

            public int Height {
                get {
                    return bottomRight.Y - topLeft.Y;
                }
            }

            private bool IsCoordinateLiesWithin(int x, int y) {
                return IsCoordinateLiesWithin(new Coordinate(x, y));
            }

            private bool IsCoordinateLiesWithin(Coordinate point) {
                return topLeft.X <= point.X && topLeft.Y <= point.Y && bottomRight.X >= point.X && bottomRight.Y >= point.Y;
            }

            public bool IntersectsRoomAndOneTileMargin(Room anotherRoom) {
                bool doIntersect = false;
                // ±1 because of Minkovsky. Just provides beforementioned margin of 1 tile.
                int x = topLeft.X - 1;
                int y = topLeft.Y - 1;
                int xRight = bottomRight.X + 1;
                int yBottom = bottomRight.Y + 1;

                // if one of the points above lies within another room, they intersect each other.
                doIntersect = IsCoordinateLiesWithin(x, y) || IsCoordinateLiesWithin(x, yBottom) || IsCoordinateLiesWithin(xRight, y) || IsCoordinateLiesWithin(xRight, yBottom);

                return doIntersect;
            }
        }

    }
}




