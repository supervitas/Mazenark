using MazeBuilder;

namespace EnemiesManager {
    public class EarthBiomeEnemySpawner : AbstractEnemySpawner {        

        private new void Start() {
            base.Start();
            InitBiomeEnemies(Biome.Earth);           
        }
    }
}