using System;
using System.Collections.Generic;
using System.Linq;
using App.EventSystem;
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
        public GameObject Floor;
        #endregion

        #region BiomeFloor
        [Header("Biome Lighting Objetcs")]
        public GameObject NightParticles;
        #endregion

        private void Awake() {
            base.Awake();
            Eventhub.Subscribe("lol1", HandleCustomEvent);
        }

        void HandleCustomEvent(object sender, EventArguments e) {
            Debug.Log( " received this message: "+ e.Message);
        }

        public override GameObject CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            GameObject parent = new GameObject();

            foreach (Edge edge in Edge.Edges) {
                var edgeMesh = Instantiate(OuterEdge, GetDefaultPositionVector(coordinate, true), edge.Rotation);
                edgeMesh.transform.parent = parent.transform;
            }
            parent.isStatic = true;
            parent.name = string.Format("Cube at {0}:{1}", coordinate.X, coordinate.Y);
            return parent;
        }

        public override GameObject CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            var go = Instantiate(Floor, GetDefaultPositionVector(coordinate, false), Quaternion.identity);
            return go;
        }

        public void PlaceLightingObjects() {
            var x = GetTileCollectionForBiome(Biome.Earth);
            Debug.Log(x.GetEnumerator().Current.tiles.Capacity);
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
