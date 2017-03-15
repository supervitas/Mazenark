using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.CubeGenerators {
	public abstract class DefaultCubeGenerator : MonoBehaviour {
	    public abstract GameObject Create(Biome biome, Coordinate coordinate, Maze maze, Vector3 whereToPlace);
	}
}

