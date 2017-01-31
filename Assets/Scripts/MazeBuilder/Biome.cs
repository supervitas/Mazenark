using System;

public class Biome
{
	private static int ID_COUNTER = 0;
	private int id = ID_COUNTER++;

	public IRoomPlacer roomPlacer;
	public IWallPlacer wallPlacer;

	public Biome(IRoomPlacer roomPlacer, IWallPlacer wallPlacer, float rarityModifier = 1.0f, float sizeModifier = 1.0f, float roomCountModifier = 1.0f, float roomSizeModifier = 1.0f) {

	}

	public static Biome Spawn = new Biome(null, null);
	public static Biome Safehouse = new Biome(null, null);
	public static Biome Lava = new Biome(null, null);
}


