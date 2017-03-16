using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
	public abstract class AbstractBiomeGenerator : MonoBehaviour {
	    public abstract GameObject CreateWall(Biome biome, Coordinate coordinate, Maze maze);
	    public abstract GameObject CreateFloor(Biome biome, Coordinate coordinate, Maze maze);

	    public Vector3 GetDefaultPositionVector(Coordinate coords, bool isWall) {
	        var vec = new Vector3 {
	            x = Utils.TransformToWorldCoordinate(coords.X),
	            y = isWall ? 0f : 0.1f,
	            z = Utils.TransformToWorldCoordinate(coords.Y)
	        };
	        return vec;
	    }
	}
}

