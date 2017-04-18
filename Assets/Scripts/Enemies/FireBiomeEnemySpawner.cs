using MazeBuilder;

namespace Enemies {
    public class FireBiomeEnemySpawner : AbstractEnemySpawner {

        private void Start() {
            SetUpEmptyTiles(Biome.Fire);
            SpawnEnemies();
            CreateEnemyBehaivor();
        }

    }
}