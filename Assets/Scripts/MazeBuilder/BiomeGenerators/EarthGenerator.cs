using System.Collections.Generic;
using App;
using App.EventSystem;
using MazeBuilder.BiomeGenerators.PlacementRules;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class EarthGenerator : AbstractBiomeGenerator {

		// should be an array...
		[SerializeField]
		private PlacementRule outerEdges;
		[SerializeField]
		private PlacementRule innerEdges;
		[SerializeField]
		private PlacementRule straightWalls;
		[SerializeField]
		private GameObject floor2;

		private readonly CollectionRandom _biomeFloors = new CollectionRandom();

        private new void Awake() {
            base.Awake();
            _biomeFloors.Add(Floor, typeof(GameObject), 1.0f);
		}

        protected override void OnNight(object sender, EventArguments args) {
            EnableParticles();
        }

        protected override void OnDay(object sender, EventArguments args) {
            DisableParticles();
        }

        private void EnableParticles() {
            foreach (var particles in ParticleList) {
                particles.Play();
            }

        }
        private void DisableParticles() {
            foreach (var particles in ParticleList) {
                particles.Stop();
            }
        }

        protected override void StartPostPlacement(object sender, EventArguments e) {
            PlaceLightingObjects();
        }

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            GameObject parent = new GameObject();

            foreach (Edge edge in Edge.Edges) {
				var edgeMeshTemplate = outerEdges.GetMeshForPlacement(maze, coordinate, edge);
				if (edgeMeshTemplate == null)
					edgeMeshTemplate = innerEdges.GetMeshForPlacement(maze, coordinate, edge);
				if (edgeMeshTemplate == null)
					edgeMeshTemplate = straightWalls.GetMeshForPlacement(maze, coordinate, edge);

				if (edgeMeshTemplate != null) {
					var edgeMesh = Instantiate(edgeMeshTemplate, GetDefaultPositionVector(coordinate), edge.Rotation);
					edgeMesh.name = string.Format(edge.Name);
					edgeMesh.transform.parent = parent.transform;
				}
            }

			// Debug walls
			//Instantiate(FlatWall, GetDefaultPositionVector(coordinate), Quaternion.identity);

			parent.isStatic = true;
            parent.name = string.Format("Cube at {0}:{1}", coordinate.X, coordinate.Y);
        }

        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            if (FloorSpawnChance >= UnityEngine.Random.Range(1, 101)) {
                Instantiate((GameObject) _biomeFloors.GetRandom(typeof(GameObject)),
                    GetDefaultPositionVector(coordinate, 0.2f), Quaternion.identity);
            }

            Instantiate(floor2, GetDefaultPositionVector(coordinate), Edge.UpRight.Rotation);

		}

        private void PlaceLightingObjects() {
            ParticleList = PlaceLightingParticles(Biome.Earth, NightParticles);
            PlaceTorches();
        }

        private void PlaceTorches() {
            var mazeTiles = AppManager.Instance.MazeInstance.Maze;

            for (var i = 0; i < mazeTiles.Width; i++) {
                for (var j = 1; j < mazeTiles.Height - 1; j++) {
                    var wall = mazeTiles[i, j];
                    if (wall.Type == Tile.Variant.Wall && mazeTiles[i, j + 1].Type != Tile.Variant.Wall
                                                       && mazeTiles[i, j - 1].Type != Tile.Variant.Wall) {
                        PlaceTorch(wall);
                    }
                }
            }
        }

        private void PlaceTorch(Tile tile) {
            if (Random.Range(0, 100) >= TorchSpawnChance) return;
            var sideOffset = Random.Range(0, 2) > 0 ? -0.53f : 0.53f;
            var rotation = sideOffset > 0 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, 270, 0);

            var position = new Vector3 {
                x = Utils.TransformToWorldCoordinate(tile.Position.X - Random.Range(-0.2f, 0.2f)),
                y = Constants.Maze.TILE_SIZE - 3.5f,
                z = Utils.TransformToWorldCoordinate(tile.Position.Y - sideOffset)
            };

            Instantiate(Torch, position, rotation);
        }

    }

}
