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

        [Header("Towers")]
        [SerializeField]
        private GameObject _tower1;
		[SerializeField]
		private GameObject _tower2;
		[SerializeField]
		private GameObject _tower3;
		[SerializeField]
		private float _chanceToSpawnTower;
		private CollectionRandom _towers = null;
		private System.Random random = new System.Random();

		/*public NatureGenerator() : base() {
			_towers = new CollectionRandom();
			_towers.Add(_tower1, typeof(GameObject));
			_towers.Add(_tower2, typeof(GameObject));
			_towers.Add(_tower3, typeof(GameObject));
		}*/

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


			if (random.NextDouble() < _chanceToSpawnTower)
				AddTower(parent, coordinate);

            parent.name = $"Cube at {coordinate.X}:{coordinate.Y}";
        }

		private void InitializeTowersCollectionIfNeeded() {
			if (_towers == null) {
				_towers = new CollectionRandom();
				_towers.Add(_tower1, typeof(GameObject));
				_towers.Add(_tower2, typeof(GameObject));
				_towers.Add(_tower3, typeof(GameObject));
			}
		}

		private void AddTower(GameObject parent, Coordinate coordinate) {
			InitializeTowersCollectionIfNeeded();
			var tower = (GameObject) _towers.GetRandom(typeof(GameObject));
			Instantiate(tower, Utils.GetDefaultPositionVector(coordinate), Quaternion.Euler(-90, random.Next(), 0));
		}

	}
}