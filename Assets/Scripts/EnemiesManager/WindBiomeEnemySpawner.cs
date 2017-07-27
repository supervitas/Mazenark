using MazeBuilder;

namespace EnemiesManager {
    public class WindBiomeEnemySpawner : AbstractEnemySpawner {
        private new void Start() {
            base.Start();
            InitBiomeEnemies(Biome.Wind);            
        }
    }
}