using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class WindGenerator : AbstractBiomeGenerator {
        #region BiomeWalls
        [Header("Biome Walls")]
        public GameObject FlatWall;
        #endregion

        #region BiomeFloor
        [Header("Biome Floor")]
        public GameObject Floor;
        #endregion

        #region BiomeFloor
        [Header("Biome Lighting Objetcs")]
        public GameObject NightParticles;
        #endregion

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            var go = Instantiate(FlatWall, GetDefaultPositionVector(coordinate, true), Quaternion.identity);
        }
        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            var go = Instantiate(Floor, GetDefaultPositionVector(coordinate, false), Quaternion.identity);
        }
    }
}