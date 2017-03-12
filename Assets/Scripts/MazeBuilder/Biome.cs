using System.Collections.Generic;
using MazeBuilder.BiomeStrategies;
using UnityEngine;
using UnityStandardAssets.Water;
using MazeBuilder.Utility;

namespace MazeBuilder {
    public class Biome {
        public float FloorYCoordinate { get; private set; }
        public float WallYCoordinate { get; private set; }
		public string Name { get; private set; }

		public static CollectionRandom allBiomes = new CollectionRandom();

		private static Biome prevPrevBiome = null;
		private static Biome prevBiome = null;
		public static Biome GetRandomBiome() {
			var randomBiome = (Biome) allBiomes.GetRandom(typeof(Biome));
			while (randomBiome == prevBiome || randomBiome == prevPrevBiome) {
				randomBiome = (Biome) allBiomes.GetRandom(typeof(Biome));
			}

			prevPrevBiome = prevBiome;
			prevBiome = randomBiome;
			System.Console.Out.WriteLine("{0} was returned!", randomBiome.Name);
			return randomBiome;
        }

        public Biome(IRoomPlacer roomPlacer, IWallPlacer wallPlacer, string name, float chanceToSpawnModifier = 1.0f, float sizeModifier = 1.0f, float roomSpawnChanceModifier = 1.0f, float roomSizeModifier = 1.0f, bool isManuallyPlaced = false) {
            WallYCoordinate = 0f;
            FloorYCoordinate = -Constants.Maze.TILE_SIZE / 2.0f + 0.1f;

            RoomPlacer = roomPlacer;
            WallPlacer = wallPlacer;
            RoomSpawnChanceModifier = roomSpawnChanceModifier;
            RoomSizeModifier = roomSizeModifier;
			Name = name;

			if (!isManuallyPlaced) {
				allBiomes.Add(new CollectionRandom.Element(this, name, typeof(Biome), chanceToSpawnModifier));
			}
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


        public static Biome Spawn = new Biome(EmptyRoomPlacer.Instance, null, "Spawn Biome", isManuallyPlaced: true);
        public static Biome Safehouse = new Biome(EmptyRoomPlacer.Instance, null, "Safehouse Biome", isManuallyPlaced: true);
        public static Biome Water = new Biome(DefaultRoomPlacer.Instance, null, "Water Biome") {FloorYCoordinate = 0.1f};
        public static Biome Earth = new Biome(DefaultRoomPlacer.Instance, null, "Earth Biome");
        public static Biome Fire = new Biome(DefaultRoomPlacer.Instance, null, "Fire Biome");
        public static Biome Wind = new Biome(DefaultRoomPlacer.Instance, null, "Wind Biome");
    }
}


