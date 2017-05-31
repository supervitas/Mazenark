using MazeBuilder;

namespace Enemies {
    public class WindBiomeEnemySpawner : AbstractEnemySpawner {
        private new void Start() {
            base.Start();
            InitEnemyPlaces(Biome.Wind);
            SpawnEnemies();
            CreateEnemyBehaivor();
            SpawnBosses();
        }
    }
}