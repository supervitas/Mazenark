using App;
using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class WindGenerator : AbstractBiomeGenerator {

        private new void Awake() {
            base.Awake();
        }

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            AppManager.Instance.InstantiateSOC(FlatWall, Utils.GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
    }
}