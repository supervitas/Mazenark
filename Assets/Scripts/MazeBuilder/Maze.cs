using System.Collections.Generic;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder {
    public class Maze {
        // First index is x-coordinate, second index is y-coordinate.
        // [0, 0] stands for topmost leftmost square.
        // 0----5----10---→ x
        // |
        // |
        // ↓
        // y

	    public Dictionary<Biome, Coordinate> BiomesSize = new Dictionary<Biome, Coordinate>();
	    public int MaxBiomeID { get; set; }


        public Maze (int width = 10, int height = 10, bool fromServer = false) {
            if (width < Constants.Maze.MIN_SIZE)
                width = Constants.Maze.MIN_SIZE;
            if (height < Constants.Maze.MIN_SIZE)
                height = Constants.Maze.MIN_SIZE;

            Tiles = new Tile[width, height];

            for (var i = 0; i < width; i++)
				for (var j = 0; j < height; j++)
					Tiles[i, j] = new Tile(new Coordinate(i, j));

            if (fromServer) {
                return;
            }

			AsRoom = new Room(new Coordinate(0, 0), new Coordinate(width - 1, height - 1));
		}

        public Tile[,] Tiles { get; }

	    public List<Coordinate> ImportantPlaces { get; } = new List<Coordinate>();

	    public List<Room> Rooms { get; } = new List<Room>();

	    public List<Room> Spawns { get; } = new List<Room>();

	    public int Width => Tiles.GetLength(0);

	    public int Height => Tiles.GetLength(1);

	    public Room AsRoom {
			get; private set;
		}

		public List<TileCollection> Biomes { get; private set; } = null;

	    public void CutPassage(Coordinate topLeft, Coordinate bottomRight) {
            CutWalls(new Room(topLeft, bottomRight), type:Tile.Variant.Empty);
        }

		public Tile this[Coordinate point] {
			get {
				return Tiles[point.X, point.Y];
			}
			private set {
				Tiles[point.X, point.Y] = value;
			}
		}

		public Tile this[int x, int y] {
			get {
				return Tiles[x, y];
			}
			private set {
				Tiles[x, y] = value;
			}
		}

		public void CutWalls(Room room, Biome fillWith = null, Tile.Variant type = Tile.Variant.Room) {
		    if (fillWith != null && !BiomesSize.ContainsKey(fillWith)) {
		        BiomesSize.Add(fillWith, new Coordinate(0, 0));
		    }
            for (var i = room.TopLeftCorner.X; i <= room.TopRightCorner.X; i++)
				for (var j = room.TopLeftCorner.Y; j <= room.BottomLeftCorner.Y; j++) {
					Tiles[i, j].Type = type;
				    if (fillWith != null) {
				        Tiles[i, j].Biome = fillWith;
				    }
				}
        }

		public bool IsPointWithin(int x, int y) {
			return x >= 0 && x < Width && y >= 0 && y < Height;
		}
		public bool IsPointWithin(Coordinate point) {
			return IsPointWithin(point.X, point.Y);
		}
		public List<Tile> GetTilesWithinRoom(Room room) {
			var tiles = new List<Tile>();

			for (var i = room.TopLeftCorner.X; i <= room.TopRightCorner.X; i++) {
				for (var j = room.TopLeftCorner.Y; j <= room.BottomLeftCorner.Y; j++) {
					if (IsPointWithin(i, j)) {
						tiles.Add(Tiles[i, j]);
					}
				}
			}

			return tiles;
		}

		public class TileCollection {
			public readonly Biome biome;
			public readonly List<Tile> tiles = new List<Tile>();
			public readonly List<Room> rooms = new List<Room>();

			public TileCollection(Biome biome) {
				this.biome = biome;
			}
		}

		public void GenerateBiomesList() {
			Biomes = new List<TileCollection>();
			int tmpCounter = 0;
			// TODO: rewrite algorithm to be not using maxBiomeID.
			MaxBiomeID = 20; // 20 biomes should be enough. 640k even better.
					
			for (int i = 0; i <= MaxBiomeID; i++) {
				TileCollection biome = null;

		
				foreach (Tile tile in Tiles) {
					if (tile.BiomeID == i) {
						if (biome == null) {
							biome = new TileCollection(tile.Biome);
							Biomes.Add(biome);
						}
						biome.tiles.Add(tile);
					}
				}

				foreach (Room room in Rooms) {
					var tile = this[room.Center];
					if (tile.BiomeID == i) {																										
						biome.rooms.Add(room);
						tmpCounter++;			
					}
				}
			}			

		}

	}
}