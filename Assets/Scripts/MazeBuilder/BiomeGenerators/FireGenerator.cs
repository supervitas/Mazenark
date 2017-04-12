using System.Collections.Generic;
using App;
using App.EventSystem;
using MazeBuilder.Utility;
using MazeBuilder.BiomeGenerators.PlacementRules;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class FireGenerator : AbstractBiomeGenerator {

		// should be in AbstractBiomeGenerator, maybe?
		[Header("Biome Placing Rules")]
		[SerializeField]
		private PlacementRule outerEdges;
		[SerializeField]
		private PlacementRule innerEdges;
		[SerializeField]
		private PlacementRule straightWalls;

		[Header("Biome floors")]
		[SerializeField]
		private GameObject floor2;

		private new void Awake() {
            base.Awake();
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
            ParticleList = PlaceLightingParticles(Biome.Fire, NightParticles);
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
					var edgeMesh = AppManager.Instance.InstantiateSOC(edgeMeshTemplate, Utils.GetDefaultPositionVector(coordinate), edge.Rotation);
					edgeMesh.name = string.Format(edge.Name);
					edgeMesh.transform.parent = parent.transform;
				}
			}

			parent.name = string.Format("Cube at {0}:{1}", coordinate.X, coordinate.Y);
		}

		public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
			if (FloorEnviromentSpawnChance >= Random.Range(1, 100)) {
			    AppManager.Instance.InstantiateSOC((GameObject) BiomeFloorsEnviroment.GetRandom(typeof(GameObject)),
					Utils.GetDefaultPositionVector(coordinate, 0.2f), Quaternion.identity);
			}

		    AppManager.Instance.InstantiateSOC(floor2, Utils.GetDefaultPositionVector(coordinate), Edge.UpRight.Rotation);
		}
    }
}