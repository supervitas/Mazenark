using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators.PlacementRules {
    internal abstract class PlacementRule : MonoBehaviour {
		[Tooltip("List of meshes can be placed on tile edge with this placement rule")]
		[SerializeField]
		//public CollectionRandom MeshesCanBePlaced = new CollectionRandom();
		/// temporary.
		protected GameObject mesh;

		public GameObject GetMeshForPlacement(Maze maze, Coordinate where, Edge whereExactly) {
			//var randomMesh = (GameObject) MeshesCanBePlaced.GetRandom(typeof(GameObject));
			var randomMesh = mesh;
			if (mesh != null && CanPlaceSomething(maze, where, whereExactly, true)) {
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
