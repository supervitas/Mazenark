using App;
using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class SpawnGenerator : AbstractBiomeGenerator {
        #region BiomeWalls
        [Header("Spawn")]
        public GameObject Spawn;
        #endregion

        private new void Awake() {
            base.Awake();
        }

        protected override void OnNight(object sender, EventArguments args) {}
        protected override void OnDay(object sender, EventArguments args) {}
        protected override void StartPostPlacement(object sender, EventArguments e) {}


        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {}

        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            AppManager.Instance.InstantiateSOC(Spawn, Utils.GetDefaultPositionVector(coordinate, 0.1f), Quaternion.identity);
        }
    }
}