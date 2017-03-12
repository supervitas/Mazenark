using System;
using MazeBuilder.Utility;
using UnityEngine;
using Random = System.Random;

namespace MazeBuilder.BiomeStrategies {
    public class DefaultRoomPlacer : IRoomPlacer {
        private static DefaultRoomPlacer instance = new DefaultRoomPlacer();
        private const int MAX_PLACEMENT_ATTEMPTS = 10;
        private static Random random = new Random();

        private DefaultRoomPlacer() {

        }

        public static IRoomPlacer Instance {
            get {
                return instance;
            }
        }


        //TODO This code can accidently spawn rooms in safehouse! It shan't be!
        public Maze PlaceRoom(Maze maze, int x, int y, int chunkLeftBoundary,
            int chunkRightBoundary, int chunkTopBoundary, int chunkBottomBoundary) {
            Biome targetBiome = maze.Tiles[x, y].Biome;

            var width = GetRandomRoomDimension(targetBiome);
            var height = GetRandomRoomDimension(targetBiome);

            var attempt = 0;
            var room = new Room(x, y, x + width - 1, y + height - 1);

            while (!CanPlaceRoomHere(maze, chunkLeftBoundary, chunkRightBoundary, chunkTopBoundary, chunkBottomBoundary, room)
                   && (attempt < MAX_PLACEMENT_ATTEMPTS)) {
                attempt++;

                width = GetRandomRoomDimension(targetBiome);
                height = GetRandomRoomDimension(targetBiome);
                x = random.Next(chunkLeftBoundary, chunkRightBoundary + 1);
                y = random.Next(chunkTopBoundary, chunkBottomBoundary + 1);

                room = new Room(x, y, x + width - 1, y + height - 1);
            }

            if (attempt >= MAX_PLACEMENT_ATTEMPTS)
                return maze;
            // else:

            maze.Rooms.Add(room);
            maze.ImportantPlaces.Add(room.Center);
            // targetBiome has not changed when room re-generates? Okay...

            maze.CutWalls(room, targetBiome);

            return maze;
        }

        private int GetRandomRoomDimension(Biome biome) {
            return (int) Math.Round(random.Next(Constants.Biome.ROOM_MIN_SIZE, Constants.Biome.ROOM_MAX_SIZE + 1) * biome.RoomSizeModifier);
        }

        private bool CanPlaceRoomHere(Maze maze, int chunkLeftBoundary, int chunkRightBoundary, int chunkTopBoundary, int chunkBottomBoundary, Room roomToTest) {
            int x = roomToTest.TopLeftCorner.X;
            int y = roomToTest.TopLeftCorner.Y;
            int xRight = roomToTest.BottomRightCorner.X;
            int yBottom = roomToTest.BottomRightCorner.Y;

            // Check if we are still within boundaries:
            // Boundaries are inclusive. At least so was said in IRoomPlacer on 14.02.2017
            if (xRight > chunkRightBoundary || yBottom > chunkBottomBoundary || /*unlikely*/ x < chunkLeftBoundary || y < chunkTopBoundary)
                return false;

            // Check if we are still within world:
            if (xRight > maze.Width - 1 || yBottom > maze.Height - 1 || /*unlikely*/ x < 0 || y < 0)
                return false;

            // Assume safehouse and spawns are rooms too...
            // Check if we are not intersecting other rooms AND safe zone of 1 tile around them:
            foreach (Room anotherRoom in maze.Rooms) {
                if (roomToTest.IntersectsRoomAndOneTileMargin(anotherRoom))
                    return false;
            }
            return true;
        }
    }
}
