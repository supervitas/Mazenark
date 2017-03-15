using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeBuilder.Utility;

namespace MazeBuilder.BiomeStrategies {
	public class EmptyTileWeighter : ITileWeighter {
		private static readonly EmptyTileWeighter instance = new EmptyTileWeighter();
		private EmptyTileWeighter() { }
		public static ITileWeighter Instance {
			get {
				return instance;
			}
		}
		
		public Maze SetTileWeight(Maze maze, Coordinate tile) {
			maze[tile].GraphWeight = int.MaxValue;

			return maze;
		}

	}
}
