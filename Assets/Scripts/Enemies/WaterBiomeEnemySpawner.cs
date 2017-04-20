using MazeBuilder;

namespace Enemies {
    public class WaterBiomeEnemySpawner : AbstractEnemySpawner {

        private new void Start() {
            base.Start();
            SetUpEmptyTiles(Biome.Water);
            SpawnEnemies();
            CreateEnemyBehaivor();
        }
    }
}