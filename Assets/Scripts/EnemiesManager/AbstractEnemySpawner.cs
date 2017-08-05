using System;
using System.Collections.Generic;
using System.Linq;
using Controls;
using Controls.Bosses;
using GameSystems;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace EnemiesManager {
    public abstract class AbstractEnemySpawner : NetworkBehaviour {
        #region BiomeEnemies
        [SerializeField]
        [Range(0, 100f)] protected float EnemySpawnChance = 25f;
        [SerializeField]
        [Range(0, 100f)] protected float EnemyIdleBehaivor = 25f;
        [Header("Biome Enemies Prefabs")]
        public GameObject[] Enemies;
        public GameObject[] Bosses;
        #endregion

        protected readonly CollectionRandom BiomeEnemies = new CollectionRandom();
        protected readonly CollectionRandom BiomeBosses = new CollectionRandom();
        protected List<Tile> EmptyTiles;
        protected List<Room> Rooms;
        protected List<GameObject> SpawnedEnemies = new List<GameObject>();
        
        private static List<Transform> _playersTransform;

        protected void Start() {            
            if (!isServer) return;
            
            SetUpRandomEnemies();
            _playersTransform = FindObjectOfType<GameManager>().GetPlayersTransforms();
            InvokeRepeating(nameof(CheckAndActivateEnemies), 0f, 1.5f);
        }

        protected void InitBiomeEnemies(Biome biomeType) {            
            CreateEnemyPlaces(biomeType);
            SpawnEnemies();
            CreateEnemyBehaivor();
            SpawnBosses();                        
        }

        private void SetUpRandomEnemies() {
            foreach (var enemy in Enemies) {
                BiomeEnemies.Add(enemy, typeof(GameObject));
            }
            foreach (var boss in Bosses) {
                BiomeBosses.Add(boss, typeof(GameObject));
            }
        }

        protected void CreateEnemyPlaces(Biome biomeType) {
            EmptyTiles = (from biome in App.AppManager.Instance.MazeInstance.Maze.Biomes
                where biome.biome == biomeType
                from tile in biome.tiles
                where tile.Type == Tile.Variant.Empty
                select tile).ToList();
            
            Rooms = (from biome in App.AppManager.Instance.MazeInstance.Maze.Biomes
                where biome.biome == biomeType
                from room in biome.rooms
                select room).ToList();
        }

        protected void SpawnEnemies() {
            foreach (var emptyTile in EmptyTiles) {
                if (!(EnemySpawnChance >= Random.Range(1, 100))) continue;
                var enemy = (GameObject) BiomeEnemies.GetRandom(typeof(GameObject));
                var inst = Instantiate(enemy, Utils.GetDefaultPositionVector(emptyTile.Position, 0.5f), Quaternion.identity);
                NetworkServer.Spawn(inst);
                SpawnedEnemies.Add(inst);
            }
        }

        protected void SpawnBosses() {
            foreach (var room in Rooms) {
                var bossGo = (GameObject) BiomeBosses.GetRandom(typeof(GameObject));
                
                var boss = Instantiate(bossGo, Utils.GetDefaultPositionVector(room.Center, 0.5f), Quaternion.identity);
                
                boss.GetComponent<BasicBossControl>().SetSpawnRoom(room);
                NetworkServer.Spawn(boss);
                SpawnedEnemies.Add(boss);
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
                var controller = enemy.GetComponent<BasicEnemyControl>();
                
                if (EnemyIdleBehaivor >= Random.Range(1, 100)) {
                    controller.AddPatroolPoint(enemy.transform.position); // add current position for enemy to go back if player will no longer visible
                    continue;
                }

                var enemyPos = Utils.TransformWorldToLocalCoordinate(enemy.transform.position.x, enemy.transform.position.z);

                foreach (var patroolPoint in GetRandomNearCoordinates(enemyPos, 3)) {
                    controller.AddPatroolPoint(patroolPoint);
                }
                
                controller.SetPatrool(true);
            }
        }

        private void CheckAndActivateEnemies() {           
            foreach (var enemy in SpawnedEnemies) {
                if (enemy == null) continue;
                
                foreach (var playerTransform in _playersTransform) {
                    if(playerTransform == null) continue;
                    
                    var distance = Vector3.Distance(playerTransform.position, enemy.transform.position);

                    if (distance < 100f) {
                        enemy.SetActive(true);
                        break;
                    }
                    
                    if (distance > 100f) {
                        enemy.SetActive(false);
                    }
                }
            }                        
        }
    }
}