using System;
using System.Collections.Generic;

public class Biome
{
	private static int ID_COUNTER = 0;
	private int id = ID_COUNTER++;

    private static float totalRandom = 0;
    private static List<RandomRange> randomRanges = new List<RandomRange>();
    private static Random random = new Random();

	private IRoomPlacer roomPlacer;
	private IWallPlacer wallPlacer;
    private float roomSpawnChanceModifier;

    public static Biome getRandomBiome()
    {
        float randomValue = (float) random.NextDouble() * totalRandom;
        Biome selectedBiome = randomRanges[0].type;
        // Are you sure it traverses from 0.0f to totalRnadom, not backwards?..
        for (int i = 0; i < randomRanges.Count; i++)
        {
            selectedBiome = randomRanges[i].type;
            if (randomRanges[i].range > randomValue)
                break;
        }
        return selectedBiome;
    }

	public Biome(IRoomPlacer roomPlacer, IWallPlacer wallPlacer, float chanceToSpawnModifier = 1.0f, float sizeModifier = 1.0f, float roomSpawnChanceModifier = 1.0f, float roomSizeModifier = 1.0f, bool isManuallyPlaced = false) {
        this.roomPlacer = roomPlacer;
        this.wallPlacer = wallPlacer;
        this.roomSpawnChanceModifier = roomSpawnChanceModifier;

        if (!isManuallyPlaced)
        {
            totalRandom += chanceToSpawnModifier;
            randomRanges.Add(new RandomRange(this, chanceToSpawnModifier));
        }
	}

    public IRoomPlacer RoomPlacer
    {
        get
        {
            return roomPlacer;
        }
    }

    public IWallPlacer WallPlacer
    {
        get
        {
            return wallPlacer;
        }
    }

    public float RoomSpawnChanceModifier
    {
        get
        {
            return roomSpawnChanceModifier;
        }
    }

    private class RandomRange
    {
        public Biome type;
        public float range;
        public RandomRange(Biome type, float range)
        {
            this.type = type;
            this.range = range;
        }
    }

	public static Biome Spawn = new Biome(null, null, isManuallyPlaced: true);
	public static Biome Safehouse = new Biome(null, null, isManuallyPlaced: true);
	public static Biome Water = new Biome(null, null);
    public static Biome Earth = new Biome(null, null);
    public static Biome Fire = new Biome(null, null);
    public static Biome Wind = new Biome(null, null);
}


