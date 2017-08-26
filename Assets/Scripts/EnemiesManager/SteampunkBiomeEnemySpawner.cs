using MazeBuilder;

namespace EnemiesManager {
    public class SteampunkBiomeEnemySpawner : AbstractEnemySpawner {
        public override void OnStartServer() {
            base.OnStartServer();            
            InitBiomeEnemies(Biome.Steampunk);            
        }
    }
}