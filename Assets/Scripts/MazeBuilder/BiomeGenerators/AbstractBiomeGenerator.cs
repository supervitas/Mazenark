using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
	public abstract class AbstractBiomeGenerator : MonoBehaviour {
	    public abstract GameObject CreateWall(Biome biome, Coordinate coordinate, Maze maze, Vector3 whereToPlace);
	    public abstract GameObject CreateFloor(Biome biome, Coordinate coordinate, Maze maze, Vector3 whereToPlace);
	}
}

