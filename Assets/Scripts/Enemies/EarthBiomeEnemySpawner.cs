using Controls;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies {
    public class EarthBiomeEnemySpawner : AbstractEnemySpawner {

        private void Start() {
            SetUpEmptyTiles(Biome.Earth);
            SpawnEnemies();
            MakePatroolPointsToEnemies();
        }

        private void MakePatroolPointsToEnemies() {
            var rand = new System.Random();
            foreach (var enemy in SpawnedEnemies) {
                var controller = enemy.GetComponent<EnemyController>();
                // Add random point to patroll from list of empty tiles
                controller.Points.Add(Utils.TransformToWorldCoordinate(EmptyTiles[rand.Next(EmptyTiles.Count)].Position));
                controller.Points.Add(Utils.TransformToWorldCoordinate(EmptyTiles[rand.Next(EmptyTiles.Count)].Position));
                controller.Points.Add(Utils.TransformToWorldCoordinate(EmptyTiles[rand.Next(EmptyTiles.Count)].Position));
                controller.PointsReady = true;
//                controller.GotoNextPoint();
            }
        }
    }
}