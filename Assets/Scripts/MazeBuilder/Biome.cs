using System;
using System.Collections.Generic;

public class Biome{
	private static int ID_COUNTER = 0;
	private int id = ID_COUNTER++;

    private static float totalRandom = 0;
    private static List<RandomRange> randomRanges = new List<RandomRange>();
    private static Random random = new Random();

    public static Biome GetRandomBiome() {
        var randomValue = (float) random.NextDouble() * totalRandom;
        var selectedBiome = randomRanges[0].type;
        // Are you sure it traverses from 0.0f to totalRnadom, not backwards?..
        foreach (var t in randomRanges) {
            selectedBiome = t.type;
            if (t.range > randomValue)
                break;
        }
        return selectedBiome;
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


