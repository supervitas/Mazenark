using MazeBuilder.BiomeGenerators.PlacementRules;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class EarthGenerator : AbstractBiomeGenerator {

		// should be an array...
        [Header("Biome Placing Rules")]
		[SerializeField]
		private PlacementRule _outerEdges;
		[SerializeField]
		private PlacementRule _innerEdges;
		[SerializeField]
		private PlacementRule _straightWalls;


        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            GameObject parent = new GameObject();

            foreach (Edge edge in Edge.Edges) {
				var edgeMeshTemplate = _outerEdges.GetMeshForPlacement(maze, coordinate, edge);
				if (edgeMeshTemplate == null)
					edgeMeshTemplate = _innerEdges.GetMeshForPlacement(maze, coordinate, edge);
				if (edgeMeshTemplate == null)
					edgeMeshTemplate = _straightWalls.GetMeshForPlacement(maze, coordinate, edge);

				if (edgeMeshTemplate != null) {
					var edgeMesh = Instantiate(edgeMeshTemplate, Utils.GetDefaultPositionVector(coordinate), edge.Rotation);
					edgeMesh.name = string.Format(edge.Name);
					edgeMesh.transform.parent = parent.transform;
				}
            }

            parent.name = $"Cube at {coordinate.X}:{coordinate.Y}";
        }



    }

}
