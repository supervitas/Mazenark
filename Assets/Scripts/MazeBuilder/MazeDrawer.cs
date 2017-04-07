using MazeBuilder.Utility;
using App.EventSystem;
using MazeBuilder.BiomeGenerators;
using UnityEngine;

namespace MazeBuilder {
    public class MazeDrawer : MonoBehaviour {
        [Tooltip("Object to be spawned as maze blocks")]

        #region BiomeCubeGenerators
        [Header("Biome Cube Generators")]
        public SpawnGenerator SpawnBiomeGenerator;
        public SafehouseGenerator SafehouseBiomeGenerator;
        public WaterGenerator WaterBiomeGenerator;
        public EarthGenerator EarthBiomeGenerator;
        public FireGenerator FireBiomeGenerator;
        public WindGenerator WindBiomeGenerator;
        public NatureGenerator NatureGenerator;
        #endregion

        private void Start() {
            App.AppManager.Instance.EventHub.Subscribe("MazeCreated", CreateMaze, this);
        }

        private void CreateMaze(object sender, EventArguments eventArguments) {
            var maze = App.AppManager.Instance.MazeInstance.Maze;
            for (var i = 0; i < maze.Width; i++) {
                for (var j = 0; j < maze.Height; j++) {
                    var generator = GetGenerator(maze.Tiles[i, j].Biome);

                    if (maze.Tiles[i, j].Type == Tile.Variant.Wall) {
                        generator.CreateWall(maze.Tiles[i, j].Biome, new Coordinate(i, j), maze);
                    } else {
                        generator.CreateFloor(maze.Tiles[i, j].Biome, new Coordinate(i, j), maze);
                    }
                }
            }
        }

        private AbstractBiomeGenerator GetGenerator(Biome biome) {
			if (biome == Biome.Spawn) {
				return SpawnBiomeGenerator;
			}
			if (biome == Biome.Safehouse) {
				return SafehouseBiomeGenerator;
			}
			if (biome == Biome.Water) {
				return WaterBiomeGenerator;
			}
			if (biome == Biome.Earth) {
				return EarthBiomeGenerator;
			}
			if (biome == Biome.Fire) {
				return FireBiomeGenerator;
			}
			if (biome == Biome.Wind) {
				return WindBiomeGenerator;
			}
            if (biome == Biome.Nature) {
                return NatureGenerator;
            }
			return EarthBiomeGenerator; //default is Earth
		}

    }
}
