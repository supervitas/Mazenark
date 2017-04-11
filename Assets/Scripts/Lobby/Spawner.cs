using System.Linq;
using App;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace Lobby {
    public class Spawner: NetworkBehaviour {
        #region BiomeEnemies
        [SerializeField]
        [Range(0, 100f)] protected float EnemySpawnChance = 25f;
        [Header("Biome Enemies Prefabs")]
        public GameObject[] Enemies;
        #endregion

        protected readonly CollectionRandom BiomeEnemies = new CollectionRandom();

        void Start() {
           SetUpRandomEnemies();

           var emptyTiles = from biome in AppManager.Instance.MazeInstance.Maze.Biomes
               from tile in biome.tiles
               where tile.Type == Tile.Variant.Empty
               select tile;

            foreach (var tile in emptyTiles) {
                if (EnemySpawnChance >= Random.Range(1, 100)) {
                    var enemy = (GameObject) BiomeEnemies.GetRandom(typeof(GameObject));
                    var inst = Instantiate(enemy, Utils.GetDefaultPositionVector(tile.Position), Quaternion.identity);
                    NetworkServer.Spawn(inst);
                }
            }
        }

        void SetUpRandomEnemies() {
            foreach (var enemy in Enemies) {
                BiomeEnemies.Add(enemy, typeof(GameObject));
            }
        }
    }
}