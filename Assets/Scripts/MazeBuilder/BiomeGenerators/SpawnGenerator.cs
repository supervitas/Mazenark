using App;
using App.EventSystem;
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

        protected override void OnNight(object sender, EventArguments args) {}
        protected override void OnDay(object sender, EventArguments args) {}

        protected override void StartPostPlacement(object sender, EventArguments e) {
            var spawns = GetTilesByTypeAndBiome(Biome.Spawn, Tile.Variant.Empty);

            foreach (var spawn in spawns) {
                Instantiate(SpawnEffect, new Vector3(
                    Utils.TransformToWorldCoordinate(spawn.Position.X),
                    45f,
                    Utils.TransformToWorldCoordinate(spawn.Position.Y)
                ), Quaternion.identity);
            }
        }
        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {}


        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            Instantiate(Spawn, Utils.GetDefaultPositionVector(coordinate, 0.1f), Quaternion.identity);
        }
    }
}