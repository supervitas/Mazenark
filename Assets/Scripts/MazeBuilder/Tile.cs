using MazeBuilder.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace MazeBuilder {
    public class Tile {
        public Biome Biome { get; set; }
        public Variant Type { get; set; }
		public int GraphWeight { get; set; }
		public Coordinate Position { get; private set; }
		public int BiomeID { get; set; }

		private Dictionary<Edge, bool> edgeOccupancy = new Dictionary<Edge, bool>();

		public Tile(Coordinate position) {
			Type = Variant.Wall;
			Position = position;

			foreach (Edge edge in Edge.Edges) {
				edgeOccupancy.Add(edge, false);
			}
		}

		public bool EdgeOccupied(Edge edge) {
			return edgeOccupancy[edge];         // I will be surprised if it throws an exception, because all Edge objects are static!
		}

		public void OccupyEdge(Edge edge) {
			edgeOccupancy[edge] = true;
		}

        public enum Variant {
            Empty,      // walkable
            Wall,       // non-passable
            Room		// others. :)     I.e. teleporters, shops, safehouses, etc.     Not sure if needed. May be replaced with Empty type instead...
        }

	}
}
