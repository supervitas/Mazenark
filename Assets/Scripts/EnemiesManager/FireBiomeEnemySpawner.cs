using MazeBuilder;

namespace EnemiesManager {
    public class FireBiomeEnemySpawner : AbstractEnemySpawner {

        private new void Start() {
            base.Start();
            InitBiomeEnemies(Biome.Fire);           
        }

    }
}