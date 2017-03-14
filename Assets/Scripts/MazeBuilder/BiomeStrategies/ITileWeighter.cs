using MazeBuilder;
using MazeBuilder.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeBuilder.BiomeStrategies {
	public interface ITileWeighter {
		// stateless!
		Maze SetTileWeight(Maze maze, Coordinate tile);
	}
}
