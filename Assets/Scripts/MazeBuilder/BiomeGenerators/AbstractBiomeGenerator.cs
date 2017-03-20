﻿using System.Collections.Generic;
using System.Linq;
using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
	public abstract class AbstractBiomeGenerator : MonoBehaviour {
	    public abstract void CreateWall(Biome biome, Coordinate coordinate, Maze maze);
	    public abstract void CreateFloor(Biome biome, Coordinate coordinate, Maze maze);
	    protected List<Maze.TileCollection> BiomesCollecton;
	    protected Publisher Eventhub;
	    protected readonly CollectionRandom ChancesToSpawnFloors = new CollectionRandom();
	    protected readonly List<ParticleSystem> ParticleList = new List<ParticleSystem>();


	    protected void Awake() {
	        BiomesCollecton = App.AppManager.Instance.MazeInstance.Maze.Biomes;
	        Eventhub = App.AppManager.Instance.EventHub;
	        ChancesToSpawnFloors.Add(false, "chanse", typeof(bool), 5);
	        ChancesToSpawnFloors.Add(true, "chanse", typeof(bool), 2);
	        App.AppManager.Instance.EventHub.Subscribe("TOD:nightStarted", OnNight, this);
	        App.AppManager.Instance.EventHub.Subscribe("TOD:dayStarted", OnDay, this);
	    }

	    protected abstract void OnNight(object sender, EventArguments args);
	    protected abstract void OnDay(object sender, EventArguments args);

	    protected void OnDestroy() {
	        Eventhub.UnsubscribeFromAll(this);
	    }

	    protected IEnumerable<Maze.TileCollection> GetTileCollectionForBiome(Biome type) {
	        return from biome in BiomesCollecton where biome.biome == type select biome;
	    }

	    protected IEnumerable<Tile> GetTilesByTypeAndBiome(Biome type, Tile.Variant tileType) {
	        return from biome in BiomesCollecton
	            where biome.biome == type
	            from tile in biome.tiles
	            where tile.Type == tileType
	            select tile;
	    }

	    protected Vector3 GetDefaultPositionVector(Coordinate coords, float y = 0f) {
	        return new Vector3 {
	            x = Utils.TransformToWorldCoordinate(coords.X),
	            y = y,
	            z = Utils.TransformToWorldCoordinate(coords.Y)
	        };
	    }

	}
}

