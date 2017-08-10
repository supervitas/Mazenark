using MazeBuilder;

namespace EnemiesManager {
    public class WaterBiomeEnemySpawner : AbstractEnemySpawner {

        public override void OnStartServer() {  
            base.OnStartServer();
            InitBiomeEnemies(Biome.Water);        
        }
    }
}