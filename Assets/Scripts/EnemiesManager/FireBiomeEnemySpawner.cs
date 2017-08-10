using MazeBuilder;

namespace EnemiesManager {
    public class FireBiomeEnemySpawner : AbstractEnemySpawner {

        public override void OnStartServer() {  
            base.OnStartServer();
            InitBiomeEnemies(Biome.Fire);           
        }

    }
}