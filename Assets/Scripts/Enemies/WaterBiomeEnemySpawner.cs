using MazeBuilder;

namespace Enemies {
    public class WaterBiomeEnemySpawner : AbstractEnemySpawner {

        private void Start() {
            SetUpEmptyTiles(Biome.Water);
            SpawnEnemies();
            CreateEnemyBehaivor();
        }
    }
}