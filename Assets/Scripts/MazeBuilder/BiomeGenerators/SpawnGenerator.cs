using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class SpawnGenerator : AbstractBiomeGenerator {

        private readonly CollectionRandom _biomeFloors = new CollectionRandom();

        private new void Awake() {
            base.Awake();
            _biomeFloors.Add(Floor, typeof(GameObject), 1.0f);

        }

        protected override void OnNight(object sender, EventArguments args) {}
        protected override void OnDay(object sender, EventArguments args) {}
        protected override void StartPostPlacement(object sender, EventArguments e) {}


        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            Instantiate(FlatWall, GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            if (FloorSpawnChance >= UnityEngine.Random.Range(1, 101)) {
                Instantiate((GameObject) _biomeFloors.GetRandom(typeof(GameObject)),
                    GetDefaultPositionVector(coordinate, 0.1f), Quaternion.identity);
            }
        }
    }
}