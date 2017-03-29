using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MazeBuilder.Utility {
	internal class Edge {
		public static List<Edge> Edges = new List<Edge>();

		public Quaternion Rotation {
			get; private set;
		}

		private Edge(Quaternion rotation) {
			Rotation = rotation;
			Edges.Add(this);
		}

		public static Edge UpRight = new Edge(Quaternion.Euler(-90, 90, 0));
		public static Edge UpLeft = new Edge(Quaternion.Euler(-90, 180, 0));
		public static Edge DownLeft = new Edge(Quaternion.Euler(-90, 270, 0));
		public static Edge DownRight = new Edge(Quaternion.Euler(-90, 360, 0));
	}
}
