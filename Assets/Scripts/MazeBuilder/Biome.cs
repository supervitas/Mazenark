using System.Collections.Generic;
using MazeBuilder.BiomeStrategies;
using UnityEngine;
using UnityStandardAssets.Water;

namespace MazeBuilder {
    public class Biome {
        private static float _totalRandom;
        private static List<RandomRange> randomRanges = new List<RandomRange>();


        public float FloorYCoordinate { get; private set; }
        public float WallYCoordinate { get; private set; }


        public static Biome GetRandomBiome() {
            return randomRanges[Random.Range(0, randomRanges.Count)].type;
        }

        public Biome(IRoomPlacer roomPlacer, IWallPlacer wallPlacer,
            float chanceToSpawnModifier = 1.0f, float sizeModifier = 1.0f,
            float roomSpawnChanceModifier = 1.0f, float roomSizeModifier = 1.0f, bool isManuallyPlaced = false) {
            WallYCoordinate = 0f;
            FloorYCoordinate = -Constants.Maze.TILE_SIZE / 2.0f + 0.1f;

            RoomPlacer = roomPlacer;
            WallPlacer = wallPlacer;
            RoomSpawnChanceModifier = roomSpawnChanceModifier;
            RoomSizeModifier = roomSizeModifier;

            if (isManuallyPlaced) return;
            _totalRandom += chanceToSpawnModifier;
            randomRanges.Add(new RandomRange(this, chanceToSpawnModifier));
        }

        public IRoomPlacer RoomPlacer { get; private set; }

        public IWallPlacer WallPlacer { get; private set; }

        public float RoomSpawnChanceModifier { get; private set; }

        public float RoomSizeModifier { get; private set; }

        private class RandomRange {
            public Biome type;
            public float range;
            public RandomRange(Biome type, float range) {
                this.type = type;
                this.range = range;
            }
        }


        public static Biome Spawn = new Biome(EmptyRoomPlacer.Instance, null, isManuallyPlaced: true);
        public static Biome Safehouse = new Biome(EmptyRoomPlacer.Instance, null, isManuallyPlaced: true);
        public static Biome Water = new Biome(DefaultRoomPlacer.Instance, null) {FloorYCoordinate = 0.1f};
        public static Biome Earth = new Biome(DefaultRoomPlacer.Instance, null);
        public static Biome Fire = new Biome(DefaultRoomPlacer.Instance, null);
        public static Biome Wind = new Biome(DefaultRoomPlacer.Instance, null);
    }
}


