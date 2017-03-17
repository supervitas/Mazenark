using System.Collections.Generic;
using System.Linq;
using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
	public abstract class AbstractBiomeGenerator : MonoBehaviour {
	    public abstract GameObject CreateWall(Biome biome, Coordinate coordinate, Maze maze);
	    public abstract GameObject CreateFloor(Biome biome, Coordinate coordinate, Maze maze);
	    protected List<Maze.TileCollection> biomesCollecton;
	    protected Publisher Eventhub;

	    protected Vector3 GetDefaultPositionVector(Coordinate coords, bool isWall) {
	        return new Vector3 {
	            x = Utils.TransformToWorldCoordinate(coords.X),
	            y = isWall ? 0f : 0.1f,
	            z = Utils.TransformToWorldCoordinate(coords.Y)
	        };
	    }

	    protected void Awake() {
	        biomesCollecton = App.AppManager.Instance.MazeInstance.Maze.Biomes;
	        Eventhub = App.AppManager.Instance.EventHub;
	    }

	    protected IEnumerable<Maze.TileCollection> GetTileCollectionForBiome(Biome type) {
	        return from biome in biomesCollecton where biome.biome == type select biome;
	    }

	}
}

