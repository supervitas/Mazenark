﻿using System.Collections.Generic;
using MazeBuilder.BiomeStrategies;
using UnityEngine;

namespace MazeBuilder {
    public class Biome{
        private static int ID_COUNTER = 0;
        private int id = ID_COUNTER++;

        private static float totalRandom = 0;
        private static List<RandomRange> randomRanges = new List<RandomRange>();


        public static Biome GetRandomBiome() {
            return randomRanges[Random.Range(0, randomRanges.Count)].type;
        }

        public Biome(IRoomPlacer roomPlacer, IWallPlacer wallPlacer,
            float chanceToSpawnModifier = 1.0f, float sizeModifier = 1.0f,
            float roomSpawnChanceModifier = 1.0f, float roomSizeModifier = 1.0f, bool isManuallyPlaced = false) {

            this.RoomPlacer = roomPlacer;
            this.WallPlacer = wallPlacer;
            this.RoomSpawnChanceModifier = roomSpawnChanceModifier;
            this.RoomSizeModifier = roomSizeModifier;

            if (isManuallyPlaced) return;
            totalRandom += chanceToSpawnModifier;
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
        public static Biome Water = new Biome(DefaultRoomPlacer.Instance, null);
        public static Biome Earth = new Biome(DefaultRoomPlacer.Instance, null);
        public static Biome Fire = new Biome(DefaultRoomPlacer.Instance, null);
        public static Biome Wind = new Biome(DefaultRoomPlacer.Instance, null);
    }
}


