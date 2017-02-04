using System;

public class Tile {
	public Biome Biome = null;
    public Type type = Type.Wall;

	public Tile (){}

    public enum Type {
        Empty,      // walkable
        Wall,       // non-passable
        Structure   // others. :)     I.e. teleporters, shops, safehouses, etc.     Not sure if needed. May be replaced with Empty type instead...
    }
}
