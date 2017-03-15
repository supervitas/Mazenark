using System.Collections.Generic;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class EarthGenerator : AbstractBiomeGenerator {
        #region BiomeWalls
        [Header("Biome Walls")]
        public GameObject FlatWall;
        public GameObject OuterEdge;
        public GameObject InnerEdge;
        #endregion

        #region BiomeFloor
        [Header("Biome Floor")]
        public GameObject floor;
        #endregion



        public override GameObject CreateWall(Biome biome, Coordinate coordinate, Maze maze, Vector3 whereToPlace) {
            GameObject parent = new GameObject();

            foreach (Edge edge in Edge.Edges) {
                var edgeMesh = Instantiate(OuterEdge, whereToPlace, edge.Rotation);
                edgeMesh.transform.parent = parent.transform;
            }
            parent.isStatic = true;
            parent.name = string.Format("Cube at {0}:{1}", coordinate.X, coordinate.Y);
            return parent;
        }

        public override GameObject CreateFloor(Biome biome, Coordinate coordinate, Maze maze, Vector3 whereToPlace) {
            var go = Instantiate(floor, whereToPlace, Quaternion.identity);
            return go;
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
