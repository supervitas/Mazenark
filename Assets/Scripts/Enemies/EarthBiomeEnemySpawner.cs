using MazeBuilder;

namespace Enemies {
    public class EarthBiomeEnemySpawner : AbstractEnemySpawner {

        private void Start() {
            SetUpEmptyTiles(Biome.Earth);
            SpawnEnemies();
            CreateEnemyBehaivor();
        }

    }
}