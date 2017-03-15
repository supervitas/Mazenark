using MazeBuilder.Utility;

namespace MazeBuilder.BiomeStrategies {
	public interface ITileWeighter {
		// stateless!
		Maze SetTileWeight(Maze maze, Coordinate tile);
	}
}
