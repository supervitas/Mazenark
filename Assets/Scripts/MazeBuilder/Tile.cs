namespace MazeBuilder {
    public class Tile {
        public Biome Biome { get; set; }
        public Variant Type { get; set; }
		public int GraphWeight { get; set; }

		public Tile (){ Type = Variant.Wall; }

        public enum Variant {
            Empty,      // walkable
            Wall,       // non-passable
            Room   // others. :)     I.e. teleporters, shops, safehouses, etc.     Not sure if needed. May be replaced with Empty type instead...
        }
    }
}
