using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeBuilder.Utility;

namespace MazeBuilder.BiomeStrategies {
	public class EuclidianTileWeighter : ITileWeighter {
		private static EuclidianTileWeighter instance = new EuclidianTileWeighter();
		private static Random random = new Random();
		private const int RANDOM_FROM = 0;
		private const int RANDOM_TO = 20;

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

			//System.Console.Out.WriteLine("Tile weight is {0}", minimalWeight);
			maze[tile].GraphWeight = minimalWeight + GetRandomWeight();
			//System.Console.Out.WriteLine("Tile weight is set to {0}", maze[tile].GraphWeight);

			return maze;
		}

		private int GetRandomWeight() {
			return random.Next(RANDOM_FROM, RANDOM_TO + 1);
		}

	}
}
