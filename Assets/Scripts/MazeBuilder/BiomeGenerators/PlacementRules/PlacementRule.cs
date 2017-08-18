using MazeBuilder.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators.PlacementRules {
    internal abstract class PlacementRule : MonoBehaviour {
		[Tooltip("List of meshes can be placed on tile edge with this placement rule")]
		[SerializeField]
		protected List<GameObject> meshes;
		[Tooltip("List of spawn chances for these meshes.")]
		[SerializeField]
		protected List<float> weights;

		private CollectionRandom _meshes = null;

		private GameObject GetRandomMesh() {
			if (_meshes == null) {
				_meshes = new CollectionRandom();
				for (int i = 0; i < meshes.Count; i++) {
					_meshes.Add(meshes[i], typeof(GameObject), weights[i], $"Mesh #{i}");
				}
			}

			return (GameObject) _meshes.GetRandom(typeof(GameObject));
		}

		public GameObject GetMeshForPlacement(Maze maze, Coordinate where, Edge whereExactly) {
			//var randomMesh = (GameObject) MeshesCanBePlaced.GetRandom(typeof(GameObject));
			var randomMesh = GetRandomMesh();
			if (CanPlaceSomething(maze, where, whereExactly, true)) {
				return randomMesh;
			} else {
				return null;
			}
		}

		public abstract bool CanPlaceSomething(Maze maze, Coordinate where, Edge whereExactly, bool occupyEdges = false);

		protected void GetShiftedTileAndEdge(Maze maze, Coordinate where, Edge whereExactly, Direction rawShift, out Tile neighbour, out Edge neighbourEdge) {
			// rawShift is relative,
			// actualShift is in global coordinates
			Direction actualShift = whereExactly.RotateDirection(rawShift);

			Edge neighboursEdge;
			Direction whereNeighbourIs;
			whereExactly.ShiftByHalfTile(actualShift, out neighboursEdge, out whereNeighbourIs);

			Coordinate neighbourPosition = whereNeighbourIs.Shift(where);
			if (!maze.IsPointWithin(neighbourPosition)) {
				neighbour = null;	// should not be used afterwards.
				neighbourEdge = null;
				//return true;
			} else {
				neighbour = maze[neighbourPosition];
				neighbourEdge = neighboursEdge;
				//return !neighbour.EdgeOccupied(neighboursEdge);
			}
		}

		protected bool IsEmpty(Tile target, Tile main) {
			return target == null || target.Type != Tile.Variant.Wall || target.Biome != main.Biome;
		}

		protected bool IsWall(Tile target, Edge isOccupied, Tile main) {
			return target != null && target.Type == Tile.Variant.Wall && !target.EdgeOccupied(isOccupied) && target.Biome == main.Biome;
		}
	}
}
