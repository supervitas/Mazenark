using System.Linq;
using App.Eventhub;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class SpawnGenerator : AbstractBiomeGenerator {
        #region BiomeWalls
        [Header("Spawn")]
        public GameObject Spawn;
        public GameObject SpawnEffect;
        #endregion

        private new void Awake() {
            base.Awake();
        }

        protected override void StartPostPlacement(object sender, EventArguments e) {
            var spawns = from biome in BiomesCollecton
                where biome.biome == Biome.Spawn
                select biome;

            foreach (var spawn in spawns) {
                Instantiate(SpawnEffect, new Vector3(
                    Utils.TransformToWorldCoordinate(spawn.tiles[spawn.tiles.Count / 2].Position.X),
                    45f,
                    Utils.TransformToWorldCoordinate(spawn.tiles[spawn.tiles.Count / 2].Position.Y)
                ), Quaternion.identity);
            }
        }
        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {}


        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            Instantiate(Spawn, Utils.GetDefaultPositionVector(coordinate, 0.1f), Edge.GetRandomEdgeRotation().Rotation);
        }
    }
}