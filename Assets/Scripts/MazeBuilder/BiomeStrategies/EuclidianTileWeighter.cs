using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeBuilder.Utility;

namespace MazeBuilder.BiomeStrategies {
	public class EuclidianTileWeighter : ITileWeighter {
		private static EuclidianTileWeighter instance = new EuclidianTileWeighter();
		private EuclidianTileWeighter() { }
		public static ITileWeighter Instance {
			get {
				return instance;
			}
		}
		
		public Maze SetTileWeight(Maze maze, Coordinate tile) {
			var pois = maze.ImportantPlaces;

			var minimalWeight = int.MaxValue;
			foreach (Coordinate poi in pois) {
				var distance = tile.EuclidianDistanceTo(poi);
				if (distance < minimalWeight)
					minimalWeight = distance;
			}

			maze[tile].GraphWeight = minimalWeight;

			return maze;
		}

	}
}
