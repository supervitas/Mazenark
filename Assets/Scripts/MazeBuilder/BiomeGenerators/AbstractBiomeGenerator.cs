using System.Collections.Generic;
using System.Linq;
using App;
using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
	public abstract class AbstractBiomeGenerator : MonoBehaviour {

	    #region BiomeSpawnChances
	    [Header("Biome Spawn Chances")]
	    [SerializeField]
	    [Range(0, 100f)] protected float FloorEnviromentSpawnChance = 25f;
	    #endregion

	    #region BiomeWalls
	    [Header("Biome Walls")]
	    public GameObject FlatWall;
	    #endregion

	    #region BiomeFloor
	    [Header("Biome floors")]
	    public GameObject Floor;

	    [Header("Biome Floors Enviroment")]
	    public GameObject[] FloorsEnviroment;
	    #endregion

	    protected List<Maze.TileCollection> BiomesCollecton;
	    protected Publisher Eventhub;
	    protected Dictionary<string, CollectionRandom> SpawnObjectsChances = new Dictionary<string, CollectionRandom>();


	    protected readonly CollectionRandom BiomeFloorsEnviroment = new CollectionRandom();

	    protected void Awake() {
	        Eventhub = AppManager.Instance.EventHub;
	        BiomesCollecton = AppManager.Instance.MazeInstance.Maze.Biomes;
	        AddFloorsToRandomGenerator();
		    Eventhub.Subscribe("mazedrawer:placement_finished", StartPostPlacement, this);
	    }

	    protected void OnDestroy() {
	        Eventhub.UnsubscribeFromAll(this);
	    }

	    public virtual void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
	        if (FloorEnviromentSpawnChance >= Random.Range(1, 100)) {
	            var position = Utils.GetDefaultPositionVector(coordinate, 0.3f);
	            position.x += Random.Range(-0.7f, 0.7f);
	            position.z += Random.Range(-0.7f, 0.7f);
	            AppManager.Instance.InstantiateSOC((GameObject) BiomeFloorsEnviroment.GetRandom(typeof(GameObject)),
	                position, Quaternion.identity);
	        }

	        AppManager.Instance.InstantiateSOC(Floor, Utils.GetDefaultPositionVector(coordinate),
	            Edge.GetRandomEdgeRotation().Rotation);
	    }

	    protected virtual void StartPostPlacement(object sender, EventArguments e) {}

	    public abstract void CreateWall(Biome biome, Coordinate coordinate, Maze maze);


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

	    protected void AddFloorsToRandomGenerator() {
	        foreach (var floor in FloorsEnviroment) {
	            BiomeFloorsEnviroment.Add(floor, typeof(GameObject));
	        }
	    }
	}
}