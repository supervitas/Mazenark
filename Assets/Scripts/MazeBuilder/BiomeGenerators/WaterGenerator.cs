using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class WaterGenerator : AbstractBiomeGenerator{
        #region BiomeWalls
        [Header("Biome Walls")]
        public GameObject FlatWall;
        #endregion

        #region BiomeFloor
        [Header("Biome Floor")]
        public GameObject Floor;
        #endregion

        public override GameObject CreateWall(Biome biome, Coordinate coordinate, Maze maze, Vector3 whereToPlace) {
            var go = Instantiate(FlatWall, whereToPlace, Quaternion.identity);
            return go;
        }
        public override GameObject CreateFloor(Biome biome, Coordinate coordinate, Maze maze, Vector3 whereToPlace) {
            var go = Instantiate(Floor, whereToPlace, Quaternion.identity);
            return go;
        }
    }
}