using MazeBuilder;

namespace EnemiesManager {
    public class WindBiomeEnemySpawner : AbstractEnemySpawner {
        public override void OnStartServer() {
            base.OnStartServer();            
            InitBiomeEnemies(Biome.Wind);            
        }
    }
}