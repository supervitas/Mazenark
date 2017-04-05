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
	    [Range(0, 100f)] protected float TorchSpawnChance = 25f;
	    [SerializeField]
	    [Range(0, 100f)] protected float ParticlesSpawnChance = 25f;
	    [SerializeField]
	    [Range(0, 100f)] protected float FloorEnviromentSpawnChance = 25f;
	    #endregion

	    #region BiomeWalls
	    [Header("Biome Walls")]
	    public GameObject FlatWall;
	    #endregion

	    #region BiomeFloor
	    [Header("Biome Floors Enviroment")]
	    public GameObject[] FloorsEnviroment;
	    #endregion

	    #region BiomeLights
	    [Header("Biome Lighting Objetcs")]
	    public ParticleSystem NightParticles;
	    public GameObject Torch;
	    #endregion

	    protected List<Maze.TileCollection> BiomesCollecton;
	    protected Publisher Eventhub;
	    protected Dictionary<string, CollectionRandom> SpawnObjectsChances = new Dictionary<string, CollectionRandom>();
	    protected List<ParticleSystem> ParticleList = new List<ParticleSystem>();
	    protected readonly CollectionRandom BiomeFloorsEnviroment = new CollectionRandom();

	    protected void Awake() {
	        Eventhub = AppManager.Instance.EventHub;
	        GeneralSubscribtion();
	        AddFloorsToRandomGenerator();
	    }

	    private void SetUp(object sender, EventArguments eventArguments) {
	        BiomesCollecton = AppManager.Instance.MazeInstance.Maze.Biomes;
	        Debug.Log(AppManager.Instance.MazeInstance.Maze[1,2].Biome.Name);
	    }

	    public abstract void CreateWall(Biome biome, Coordinate coordinate, Maze maze);
	    public abstract void CreateFloor(Biome biome, Coordinate coordinate, Maze maze);
	    protected abstract void OnNight(object sender, EventArguments args);
	    protected abstract void OnDay(object sender, EventArguments args);
	    protected abstract void StartPostPlacement(object sender, EventArguments e);


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

	    protected List<ParticleSystem> PlaceLightingParticles(Biome biomeType, ParticleSystem particles) {
//	        Debug.Log(GetTilesByTypeAndBiome(biomeType, Tile.Variant.Wall).ToList().Count);
	        return (from tile in GetTilesByTypeAndBiome(biomeType, Tile.Variant.Empty)
	        let shouldPlace = ParticlesSpawnChance >= Random.Range(1, 100)
            where shouldPlace select Instantiate(particles, GetDefaultPositionVector(tile.Position, 3.5f), Quaternion.identity)).ToList();
	    }

	    private void GeneralSubscribtion() {
	        Eventhub.Subscribe("MazeLoaded", SetUp, this);
	        Eventhub.Subscribe("mazedrawer:placement_finished", StartPostPlacement, this);
	        Eventhub.Subscribe("TOD:nightStarted", OnNight, this);
            Eventhub.Subscribe("TOD:dayStarted", OnDay, this);
	    }

	    protected void AddFloorsToRandomGenerator() {
	        foreach (var floor in FloorsEnviroment) {
	            var settings = floor.GetComponent<PrefabSettings>();
	            var chance = 1.0f;
	            if (settings) {
	                chance = settings.SpawnChances;
	            }
	            BiomeFloorsEnviroment.Add(floor, typeof(GameObject), chance);
	        }
	    }

	}
}