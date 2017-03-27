using System.Collections.Generic;
using System.Linq;
using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
	public abstract class AbstractBiomeGenerator : MonoBehaviour {
	    #region BiomeLights
	    [Header("Biome Lighting Objetcs")]
	    public ParticleSystem NightParticles;
	    public GameObject Torch;
	    #endregion

	    #region BiomeLightsChances
	    [Header("Biome Spawn Chances")]
	    [SerializeField]
	    [Range(0, 100f)] public float TorchSpawnChance = 25f;
	    [Range(0, 100f)] public float ParticlesSpawnChance = 25f;
	    #endregion

	    protected List<Maze.TileCollection> BiomesCollecton;
	    protected Publisher Eventhub;
	    protected Dictionary<string, CollectionRandom> SpawnObjectsChances = new Dictionary<string, CollectionRandom>();
	    protected List<ParticleSystem> ParticleList = new List<ParticleSystem>();



	    protected void Awake() {
	        BiomesCollecton = App.AppManager.Instance.MazeInstance.Maze.Biomes;
	        Eventhub = App.AppManager.Instance.EventHub;
	        GeneralSubscribtion();
	        InitSpawnChances();
	    }

	    public abstract void CreateWall(Biome biome, Coordinate coordinate, Maze maze);
	    public abstract void CreateFloor(Biome biome, Coordinate coordinate, Maze maze);
	    protected abstract void OnNight(object sender, EventArguments args);
	    protected abstract void OnDay(object sender, EventArguments args);
	    protected abstract void StartPostPlacement(object sender, EventArguments e);

	    protected void OnDestroy() {
	        Eventhub.UnsubscribeFromAll(this);
	    }

	    protected void InitSpawnChances() {
	        SpawnObjectsChances.Add("floor", new CollectionRandom());
	        SpawnObjectsChances["floor"].Add(false, typeof(bool), 5);
	        SpawnObjectsChances["floor"].Add(true, typeof(bool), 2);

	        SpawnObjectsChances.Add("nightParticles", new CollectionRandom());
	        SpawnObjectsChances["nightParticles"].Add(false, typeof(bool), 3);
	        SpawnObjectsChances["nightParticles"].Add(true, typeof(bool), 1);

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

	    protected List<ParticleSystem> PlaceLightingParticles(Biome biomeType, ParticleSystem particles) {
	        return (from tile in GetTilesByTypeAndBiome(biomeType, Tile.Variant.Empty)
	        let shouldPlace = (bool) SpawnObjectsChances["nightParticles"].GetRandom(typeof(bool))
            where shouldPlace select Instantiate(particles, GetDefaultPositionVector(tile.Position, 3.5f), Quaternion.identity)).ToList();
	    }

	    private void GeneralSubscribtion() {
	        Eventhub.Subscribe("mazedrawer:placement_finished", StartPostPlacement, this);
	        Eventhub.Subscribe("TOD:nightStarted", OnNight, this);
            Eventhub.Subscribe("TOD:dayStarted", OnDay, this);
	    }

	}
}

