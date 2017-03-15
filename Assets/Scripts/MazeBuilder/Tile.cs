﻿using MazeBuilder.Utility;

namespace MazeBuilder {
    public class Tile {
        public Biome Biome { get; set; }
        public Variant Type { get; set; }
		public int GraphWeight { get; set; }
		public Coordinate Position { get; private set; }
		public bool WereWallsBuilt { get; set; }
		public int BiomeID { get; set; }

		public Tile(Coordinate position) {
			Type = Variant.Wall;
			Position = position;
			WereWallsBuilt = false;
		}

        public enum Variant {
            Empty,      // walkable
            Wall,       // non-passable
            Room   // others. :)     I.e. teleporters, shops, safehouses, etc.     Not sure if needed. May be replaced with Empty type instead...
        }
    }
}
