using App;
using App.Eventhub;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class SafehouseGenerator : AbstractBiomeGenerator {

        #region BiomeSafehouse
        [Header("Safehouse")]
        public GameObject Safehouse;
        #endregion

        private new void Awake() {
            base.Awake();
        }

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            Instantiate(FlatWall, Utils.GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
    }
}