using MazeBuilder;

namespace Enemies {
    public class FireBiomeEnemySpawner : AbstractEnemySpawner {

        private new void Start() {
            base.Start();
            InitEnemyPlaces(Biome.Fire);
            SpawnEnemies();
            CreateEnemyBehaivor();
            SpawnBosses();
        }

    }
}