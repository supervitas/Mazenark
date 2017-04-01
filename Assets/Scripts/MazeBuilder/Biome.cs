using MazeBuilder.BiomeStrategies;
using MazeBuilder.Utility;


namespace MazeBuilder {
    public class Biome {
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

        public Biome(IRoomPlacer roomPlacer, IWallPlacer wallPlacer, ITileWeighter tileWeighter,
            string name, float chanceToSpawnModifier = 1.0f, float sizeModifier = 1.0f,
            float roomSpawnChanceModifier = 1.0f, float roomSizeModifier = 1.0f, bool isManuallyPlaced = false) {
            RoomPlacer = roomPlacer;
            WallPlacer = wallPlacer;
			TileWeighter = tileWeighter;
            RoomSpawnChanceModifier = roomSpawnChanceModifier;
            RoomSizeModifier = roomSizeModifier;
			Name = name;
			IsManuallyPlaced = isManuallyPlaced;
			ChanceToSpawnModifier = chanceToSpawnModifier;
			SizeModifier = sizeModifier;

			if (!isManuallyPlaced) {
				allBiomes.Add(new CollectionRandom.Element(this, name, typeof(Biome), chanceToSpawnModifier));
			}
        }

        public IRoomPlacer RoomPlacer { get; private set; }

        public IWallPlacer WallPlacer { get; private set; }

		public ITileWeighter TileWeighter { get; private set; }

		public float RoomSpawnChanceModifier { get; private set; }

        public float RoomSizeModifier { get; private set; }

		public float ChanceToSpawnModifier { get; private set; }

		public float SizeModifier { get; private set; }

		public bool IsManuallyPlaced { get; private set; }

		private class RandomRange {
            public Biome type;
            public float range;
            public RandomRange(Biome type, float range) {
                this.type = type;
                this.range = range;
            }
        }


        public static Biome Spawn = new Biome(EmptyRoomPlacer.Instance, null, EmptyTileWeighter.Instance, "Spawn Biome", isManuallyPlaced: true);
        public static Biome Safehouse = new Biome(EmptyRoomPlacer.Instance, null, EmptyTileWeighter.Instance, "Safehouse Biome", isManuallyPlaced: true);
		public static Biome Water = new Biome(DefaultRoomPlacer.Instance, null, EuclidianTileWeighter.Instance, "Water Biome");
        public static Biome Earth = new Biome(DefaultRoomPlacer.Instance, null, EuclidianTileWeighter.Instance, "Earth Biome");
        public static Biome Fire = new Biome(DefaultRoomPlacer.Instance, null, EuclidianTileWeighter.Instance, "Fire Biome", chanceToSpawnModifier:0.5f, sizeModifier:0.25f);
        public static Biome Wind = new Biome(DefaultRoomPlacer.Instance, null, EuclidianTileWeighter.Instance, "Wind Biome");
        public static Biome Nature = new Biome(DefaultRoomPlacer.Instance, null, EuclidianTileWeighter.Instance, "Nature Biome");
    }
}


