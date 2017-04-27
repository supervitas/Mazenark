using App;
using App.EventSystem;
using MazeBuilder.BiomeGenerators.PlacementRules;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class NatureGenerator : AbstractBiomeGenerator {
        // should be an array...
        [Header("Biome Placing Rules")]
        [SerializeField]
        private PlacementRule _outerEdges;
        [SerializeField]
        private PlacementRule _innerEdges;
        [SerializeField]
        private PlacementRule _straightWalls;

        [Header("Biome floors")]
        [SerializeField]
        private GameObject _floor2;


        private new void Awake() {
            base.Awake();
        }


        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            GameObject parent = new GameObject();

            foreach (Edge edge in Edge.Edges) {
                var edgeMeshTemplate = _outerEdges.GetMeshForPlacement(maze, coordinate, edge);
                if (edgeMeshTemplate == null)
                    edgeMeshTemplate = _innerEdges.GetMeshForPlacement(maze, coordinate, edge);
                if (edgeMeshTemplate == null)
                    edgeMeshTemplate = _straightWalls.GetMeshForPlacement(maze, coordinate, edge);

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