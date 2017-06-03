using MazeBuilder;

namespace Enemies {
    public class WaterBiomeEnemySpawner : AbstractEnemySpawner {

        private new void Start() {
            base.Start();
            InitBiomeEnemies(Biome.Water);        
        }
    }
}