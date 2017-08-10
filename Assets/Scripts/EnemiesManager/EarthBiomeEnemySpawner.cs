using MazeBuilder;

namespace EnemiesManager {
    public class EarthBiomeEnemySpawner : AbstractEnemySpawner {        

        public override void OnStartServer() {  
            base.OnStartServer();
            InitBiomeEnemies(Biome.Earth);           
        }
    }
}