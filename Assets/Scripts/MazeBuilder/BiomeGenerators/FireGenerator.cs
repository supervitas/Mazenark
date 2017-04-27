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


		private new void Awake() {
            base.Awake();
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
    }
}