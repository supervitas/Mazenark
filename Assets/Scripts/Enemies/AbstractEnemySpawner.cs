using System.Collections.Generic;
using System.Linq;
using Controls;
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

        private void GetRandomNearCoordinates(Coordinate coordinate) {

        }

        protected void MakePatroolPointsToEnemies() {
            var rand = new System.Random();
            foreach (var enemy in SpawnedEnemies) {
                var controller = enemy.GetComponent<EnemyController>();
                var enemyPos = Utils.TransformWorldToLocalCoordinate(enemy.transform.position.x, enemy.transform.position.z);
                GetRandomNearCoordinates(enemyPos);
                // Add random point to patroll from list of empty tiles
                for (var i = 0; i < 2; i++) {
                    controller.Points.Add(
                        Utils.TransformToWorldCoordinate(EmptyTiles[rand.Next(EmptyTiles.Count)].Position)); // todo should be near points
                }

                controller.PointsReady = true;
                controller.GotoNextPoint();
            }
        }

        private void SetUpRandomEnemies() {
            foreach (var enemy in Enemies) {
                BiomeEnemies.Add(enemy, typeof(GameObject));
            }
        }
    }
}