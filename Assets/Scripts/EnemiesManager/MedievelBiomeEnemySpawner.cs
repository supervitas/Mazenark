using MazeBuilder;

namespace EnemiesManager {
    public class MedievelBiomeEnemySpawner : AbstractEnemySpawner {

        public override void OnStartServer() {  
            base.OnStartServer();
            InitBiomeEnemies(Biome.Medievel);        
        }
    }
}