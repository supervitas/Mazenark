using MazeBuilder;

namespace Enemies {
    public class WaterBiomeEnemySpawner : AbstractEnemySpawner {

        private new void Start() {
            base.Start();
            InitEnemyPlaces(Biome.Water);
            SpawnEnemies();
            CreateEnemyBehaivor();
            SpawnBosses();
        }
    }
}