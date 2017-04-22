using System;
using System.Collections.Generic;
using System.Linq;
using Controls;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Enemies {
    public class AbstractEnemySpawner : NetworkBehaviour {
        #region BiomeEnemies
        [SerializeField]
        [Range(0, 100f)] protected float EnemySpawnChance = 25f;
        [SerializeField]
        [Range(0, 100f)] protected float EnemyIdleBehaivor = 25f;
        [Header("Biome Enemies Prefabs")]
        public GameObject[] Enemies;
        #endregion

        protected readonly CollectionRandom BiomeEnemies = new CollectionRandom();
        protected List<Tile> EmptyTiles;
        protected List<GameObject> SpawnedEnemies = new List<GameObject>();

        protected void Start() {
            if (!isServer) return;
            SetUpRandomEnemies();
        }

        private void SetUpRandomEnemies() {
            foreach (var enemy in Enemies) {
                BiomeEnemies.Add(enemy, typeof(GameObject));
            }
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
                var inst = Instantiate(enemy, Utils.GetDefaultPositionVector(emptyTile.Position, 0.1f), Quaternion.identity);
                NetworkServer.Spawn(inst);
                SpawnedEnemies.Add(inst);
            }
        }

        protected Vector3 GetRandomEmptyCoordinate(Coordinate coordinate) {
            var x = coordinate.X + Random.Range(-12, 12);
            var y = coordinate.Y + Random.Range(-12, 12);
            try {
                return App.AppManager.Instance.MazeInstance.Maze[x, y].Type == Tile.Variant.Empty ?
                    Utils.TransformToWorldCoordinate(App.AppManager.Instance.MazeInstance.Maze[x, y].Position) :
                    GetRandomEmptyCoordinate(coordinate);
            }
            catch (IndexOutOfRangeException) {
                return GetRandomEmptyCoordinate(coordinate);
            }
        }

        protected IEnumerable<Vector3> GetRandomNearCoordinates(Coordinate coordinate, int coordsLimit) {
            var randomEmptyCoords = new List<Vector3> {Utils.TransformToWorldCoordinate(coordinate)}; // add first point to patrool
            for (var i = 0; i < coordsLimit; i++) {
                randomEmptyCoords.Add(GetRandomEmptyCoordinate(coordinate));
            }
            return randomEmptyCoords;
        }


        protected void CreateEnemyBehaivor() {
            foreach (var enemy in SpawnedEnemies) {
                var controller = enemy.GetComponent<EnemyController>();

                if (EnemyIdleBehaivor >= Random.Range(1, 100)) {
                    controller.SetIdleBehaivor();
                    controller.Points.Add(enemy.transform.position); // add current position for enemy to go back if player will no longer visible
                    continue;
                }

                var enemyPos = Utils.TransformWorldToLocalCoordinate(enemy.transform.position.x, enemy.transform.position.z);

                foreach (var patroolPoint in GetRandomNearCoordinates(enemyPos, 3)) {
                    controller.Points.Add(patroolPoint);
                }

                controller.CanPatrool = true;
                controller.GotoNextPoint();

            }
        }
    }
}