using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class FireGenerator : AbstractBiomeGenerator {
        #region BiomeWalls
        [Header("Biome Walls")]
        public GameObject FlatWall;
        #endregion

        #region BiomeFloor
        [Header("Biome Floor")]
        public GameObject Floor;
        #endregion


        public override GameObject CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            var go = Instantiate(FlatWall, GetDefaultPositionVector(coordinate, true), Quaternion.identity);
            return go;
        }
        public override GameObject CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            var go = Instantiate(Floor, GetDefaultPositionVector(coordinate, false), Quaternion.identity);
            return go;
        }
    }
}