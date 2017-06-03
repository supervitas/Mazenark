using MazeBuilder;
using UnityEngine;

namespace Enemies {
    public class EarthBiomeEnemySpawner : AbstractEnemySpawner {        

        private new void Start() {
            base.Start();
            InitBiomeEnemies(Biome.Earth);           
        }
    }
}