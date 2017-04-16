using MazeBuilder;

namespace Enemies {
    public class WindBiomeEnemySpawner : AbstractEnemySpawner {
        private void Start() {
            SetUpEmptyTiles(Biome.Wind);
            SpawnEnemies();
            CreateEnemyBehaivor();
        }
    }
}