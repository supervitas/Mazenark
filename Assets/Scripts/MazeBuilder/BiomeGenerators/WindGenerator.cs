using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class WindGenerator : AbstractBiomeGenerator {

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            Instantiate(FlatWall, Utils.GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
    }
}