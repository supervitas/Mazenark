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

        private bool TestEmptyCoordinate(Coordinate coordinate) {
            return App.AppManager.Instance.MazeInstance.Maze[coordinate.X, coordinate.Y].Type == Tile.Variant.Empty;
        }

        private Coordinate GetRandomCoordinate(Coordinate coordinate) {
            var x = coordinate.X + Random.Range(-8, 8);
            var y = coordinate.Y + Random.Range(-8, 8);
            try {
                return App.AppManager.Instance.MazeInstance.Maze[x, y].Position;
            }
            catch (IndexOutOfRangeException) {
                return GetRandomCoordinate(coordinate);
            }
        }

        private List<Vector3> GetRandomNearCoordinates(Coordinate coordinate, int coordsLimit) {
            Coordinate coord = GetRandomCoordinate(coordinate);
            var randomEmptyCoords = new List<Vector3>();
            randomEmptyCoords.Add(Utils.TransformToWorldCoordinate(coord)); // add first point to patrool
            for (var i = 0; i < coordsLimit; i++) {
                while (!TestEmptyCoordinate(coord)) {
                    coord = GetRandomCoordinate(coordinate);
                }
                randomEmptyCoords.Add(Utils.TransformToWorldCoordinate(coord));
            }
            return randomEmptyCoords;
        }


        protected void MakePatroolPointsToEnemies() {
            foreach (var enemy in SpawnedEnemies) {
                var controller = enemy.GetComponent<EnemyController>();
                var enemyPos = Utils.TransformWorldToLocalCoordinate(enemy.transform.position.x, enemy.transform.position.z);

                foreach (var patroolPoint in GetRandomNearCoordinates(enemyPos, 3)) {
                    controller.Points.Add(patroolPoint);
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