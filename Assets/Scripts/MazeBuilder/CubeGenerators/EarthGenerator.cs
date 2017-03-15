using MazeBuilder.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace MazeBuilder.CubeGenerators {
    public class EarthGenerator : MonoBehaviour {
        public GameObject FlatWall;
        public GameObject OuterEdge;
        public GameObject InnerEdge;

        public  GameObject Create(Biome biome, Coordinate coordinate, Maze maze, Vector3 whereToPlace) {
            GameObject parent = new GameObject();

            foreach (Edge edge in Edge.Edges) {
                var edgeMesh = Instantiate(OuterEdge, whereToPlace, edge.Rotation);
                edgeMesh.transform.parent = parent.transform;	// This should add wall as a child.
            }
            parent.isStatic = true;
            parent.name = string.Format("Cube at {0}:{1}", coordinate.X, coordinate.Y);
            return parent;
        }
    }

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
