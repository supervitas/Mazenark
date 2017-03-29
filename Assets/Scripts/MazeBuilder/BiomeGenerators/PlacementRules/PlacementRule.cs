using MazeBuilder.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
	abstract class PlacementRule : ScriptableObject {
		[Tooltip("List of meshes can be placed on tile edge with this placement rule")]
		[SerializeField]
		//public CollectionRandom MeshesCanBePlaced = new CollectionRandom();
		/// temporary.
		protected GameObject mesh;

		public abstract bool PlaceSomething(Maze maze, Coordinate where, Edge whereExactly);

		private bool IsRequestedEdgeEmpty(Maze maze, Coordinate where, Edge whereExactly, Direction rawShift, out Tile neighbour, out Edge neighbourEdge) {
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
				return true;
			} else {
				neighbour = maze[neighbourPosition];
				neighbourEdge = neighboursEdge;
				return !neighbour.EdgeOccupied(neighboursEdge);
			}
		}
	}
}
