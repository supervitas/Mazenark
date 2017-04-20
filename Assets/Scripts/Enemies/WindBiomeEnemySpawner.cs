using MazeBuilder;

namespace Enemies {
    public class WindBiomeEnemySpawner : AbstractEnemySpawner {
        private new void Start() {
            base.Start();
            SetUpEmptyTiles(Biome.Wind);
            SpawnEnemies();
            CreateEnemyBehaivor();
        }
    }
}