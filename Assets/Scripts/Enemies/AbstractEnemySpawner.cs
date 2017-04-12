using System.Collections.Generic;
using System.Linq;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace Enemies {
    public class AbstractEnemySpawner : NetworkBehaviour {
        #region BiomeEnemies
        [SerializeField]
        [Range(0, 100f)] protected float EnemySpawnChance = 25f;
        [Header("Biome Enemies Prefabs")]
        public GameObject[] Enemies;
        #endregion

        protected readonly CollectionRandom BiomeEnemies = new CollectionRandom();
        protected List<Tile> EmptyTiles;
        protected List<GameObject> SpawnedEnemies = new List<GameObject>();

        protected void Awake() {
            SetUpRandomEnemies();
        }

        protected void SetUpEmptyTiles(Biome biomeType) {
            EmptyTiles = (from biome in App.AppManager.Instance.MazeInstance.Maze.Biomes
                where biome.biome == biomeType
                from tile in biome.tiles
                where tile.Type == Tile.Variant.Empty
                select tile).ToList();
        }

        protected void SpawnEnemies() {
            foreach (var emptyTile in EmptyTiles) {
                if (!(EnemySpawnChance >= Random.Range(1, 100))) continue;
                var enemy = (GameObject) BiomeEnemies.GetRandom(typeof(GameObject));
                var inst = Instantiate(enemy, Utils.GetDefaultPositionVector(emptyTile.Position), Quaternion.identity);
                NetworkServer.Spawn(inst);

                SpawnedEnemies.Add(inst);
            }
        }

        private void SetUpRandomEnemies() {
            foreach (var enemy in Enemies) {
                BiomeEnemies.Add(enemy, typeof(GameObject));
            }
        }
    }
}