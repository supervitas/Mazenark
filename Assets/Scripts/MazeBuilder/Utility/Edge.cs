using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MazeBuilder.Utility {
	public class Edge {
		public static List<Edge> Edges = new List<Edge>();
		private Dictionary<Direction, Direction> rotatedDirections = new Dictionary<Direction, Direction>();
		private Dictionary<Direction, Direction> shiftedDirections = new Dictionary<Direction, Direction>();
		private Dictionary<Direction, Edge> shiftedEdges = new Dictionary<Direction, Edge>();
		public Quaternion Rotation {
			get; private set;
		}

		public Direction RotateDirection(Direction dir) {
			if (rotatedDirections.ContainsKey(dir)) {
				return rotatedDirections[dir];		// I will be surprised if it throws an exception, because all Direction objects are static!
			} else {
				return dir;
			}
		}

		/// Use RotateDirection only once to get correct toWhereToShift parameter!
		/// After this, there should be no need to call RotateDirection for shouldShiftToAnotherTile value!
		//	This code definetly needs some major refactoring. But it will affect Maze.cs...
		public void ShiftByHalfTile(Direction toWhereToShift, out Edge whichEdge, out Direction shouldShiftToAnotherTile) {
			if (shiftedDirections.ContainsKey(toWhereToShift)) {
				shouldShiftToAnotherTile = shiftedDirections[toWhereToShift];
			} else {
				shouldShiftToAnotherTile = Direction.Nowhere;
			}

			if (shiftedEdges.ContainsKey(toWhereToShift)) {
				whichEdge = shiftedEdges[toWhereToShift];
			} else {
				whichEdge = this;
			}
		}

		private Edge(Quaternion rotation) {
			Rotation = rotation;
			Edges.Add(this);
		}

		public static Edge UpRight = new Edge(Quaternion.Euler(-90, -90, 0));
		public static Edge UpLeft = new Edge(Quaternion.Euler(-90, 180, 0));
		public static Edge DownLeft = new Edge(Quaternion.Euler(-90, 90, 0));
		public static Edge DownRight = new Edge(Quaternion.Euler(-90, 0, 0));

		static Edge() {
			#region Rotations
			// Nothing changes.
			UpRight.rotatedDirections.Add(Direction.Up, Direction.Up);
			UpRight.rotatedDirections.Add(Direction.Right, Direction.Right);
			UpRight.rotatedDirections.Add(Direction.Left, Direction.Left);
			UpRight.rotatedDirections.Add(Direction.Bottom, Direction.Bottom);

			// CCW 90°
			UpLeft.rotatedDirections.Add(Direction.Up, Direction.Left);
			UpLeft.rotatedDirections.Add(Direction.Right, Direction.Up);
			UpLeft.rotatedDirections.Add(Direction.Left, Direction.Bottom);
			UpLeft.rotatedDirections.Add(Direction.Bottom, Direction.Right);

			// Opposite
			DownLeft.rotatedDirections.Add(Direction.Up, Direction.Bottom);
			DownLeft.rotatedDirections.Add(Direction.Right, Direction.Left);
			DownLeft.rotatedDirections.Add(Direction.Left, Direction.Right);
			DownLeft.rotatedDirections.Add(Direction.Bottom, Direction.Up);

			// CW 90°
			DownRight.rotatedDirections.Add(Direction.Up, Direction.Right);
			DownRight.rotatedDirections.Add(Direction.Right, Direction.Bottom);
			DownRight.rotatedDirections.Add(Direction.Left, Direction.Up);
			DownRight.rotatedDirections.Add(Direction.Bottom, Direction.Left);
			#endregion

			#region Shiftings
			UpRight.shiftedEdges.Add(Direction.Up, DownRight);				UpRight.shiftedDirections.Add(Direction.Up, Direction.Up);
			UpRight.shiftedEdges.Add(Direction.Right, UpLeft);				UpRight.shiftedDirections.Add(Direction.Right, Direction.Right);
			UpRight.shiftedEdges.Add(Direction.Left, UpLeft);				//
			UpRight.shiftedEdges.Add(Direction.Bottom, DownRight);			//

			UpLeft.shiftedEdges.Add(Direction.Up, DownLeft);				UpLeft.shiftedDirections.Add(Direction.Up, Direction.Up);
			UpLeft.shiftedEdges.Add(Direction.Right, UpRight);				//
			UpLeft.shiftedEdges.Add(Direction.Left, UpRight);				UpLeft.shiftedDirections.Add(Direction.Left, Direction.Left);
			UpLeft.shiftedEdges.Add(Direction.Bottom, DownLeft);			//

			DownLeft.shiftedEdges.Add(Direction.Up, UpLeft);				//
			DownLeft.shiftedEdges.Add(Direction.Right, DownRight);			//
			DownLeft.shiftedEdges.Add(Direction.Left, DownRight);			DownLeft.shiftedDirections.Add(Direction.Left, Direction.Left);
			DownLeft.shiftedEdges.Add(Direction.Bottom, UpLeft);			DownLeft.shiftedDirections.Add(Direction.Bottom, Direction.Bottom);

			DownRight.shiftedEdges.Add(Direction.Up, UpRight);				//
			DownRight.shiftedEdges.Add(Direction.Right, DownLeft);			DownRight.shiftedDirections.Add(Direction.Right, Direction.Right);
			DownRight.shiftedEdges.Add(Direction.Left, DownLeft);			//
			DownRight.shiftedEdges.Add(Direction.Bottom, UpRight);			DownRight.shiftedDirections.Add(Direction.Bottom, Direction.Bottom);
			#endregion
		}
	}
}
