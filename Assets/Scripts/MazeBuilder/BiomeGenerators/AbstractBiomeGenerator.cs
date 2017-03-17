using System.Collections.Generic;
using System.Linq;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
	public abstract class AbstractBiomeGenerator : MonoBehaviour {
	    public abstract GameObject CreateWall(Biome biome, Coordinate coordinate, Maze maze);
	    public abstract GameObject CreateFloor(Biome biome, Coordinate coordinate, Maze maze);

	    protected Vector3 GetDefaultPositionVector(Coordinate coords, bool isWall) {
	        return new Vector3 {
	            x = Utils.TransformToWorldCoordinate(coords.X),
	            y = isWall ? 0f : 0.1f,
	            z = Utils.TransformToWorldCoordinate(coords.Y)
	        };
	    }

	    protected IEnumerable<Maze.TileCollection> GetTileCollectionForBiome(Biome type) {
	       var BiomesCollecton = App.AppManager.Instance.MazeInstance.Maze.Biomes;
	        return from biome in BiomesCollecton where biome.biome == type select biome;
	    }
	}
}

