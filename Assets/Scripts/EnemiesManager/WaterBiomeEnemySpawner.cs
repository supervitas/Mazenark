using MazeBuilder;

namespace EnemiesManager {
    public class WaterBiomeEnemySpawner : AbstractEnemySpawner {

        private new void Start() {
            base.Start();
            InitBiomeEnemies(Biome.Water);        
        }
    }
}