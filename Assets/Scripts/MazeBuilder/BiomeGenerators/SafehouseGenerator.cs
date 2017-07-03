using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class SafehouseGenerator : AbstractBiomeGenerator {

        #region BiomeSafehouse
        [Header("Safehouse")]
        public GameObject Safehouse;
        #endregion

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            Instantiate(FlatWall, Utils.GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
    }
}