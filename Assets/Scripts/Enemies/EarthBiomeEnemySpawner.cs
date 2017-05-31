using MazeBuilder;
using UnityEngine;

namespace Enemies {
    public class EarthBiomeEnemySpawner : AbstractEnemySpawner {        

        private new void Start() {
            base.Start();
            InitEnemyPlaces(Biome.Earth);
            SpawnEnemies();
            CreateEnemyBehaivor();
            SpawnBosses();
        }
    }
}