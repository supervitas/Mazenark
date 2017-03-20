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
        public ParticleSystem NightParticles;
        private readonly List<ParticleSystem> _particleList = new List<ParticleSystem>();

        public GameObject Torch;
        #endregion

        private readonly CollectionRandom _biomeFloors = new CollectionRandom();

        private new void Awake() {
            base.Awake();
            Eventhub.Subscribe("mazedrawer:placement_finished", StartPostPlacement, this);
            App.AppManager.Instance.EventHub.Subscribe("TOD:nightStarted", EnableParticles, this);
            App.AppManager.Instance.EventHub.Subscribe("TOD:dayStarted", DisableParticles, this);
            _biomeFloors.Add(Floor, "earthFloors", typeof(GameObject), 1.0f);
        }


        private void EnableParticles(object caller, EventArguments args) {
            foreach (var particles in _particleList) {
                ParticleSystem.EmissionModule emission = particles.emission;
                emission.enabled = true;
            }

        }
        private void DisableParticles(object caller, EventArguments args) {
            foreach (var particles in _particleList) {
                ParticleSystem.EmissionModule emission = particles.emission;
                emission.enabled = false;
            }
        }

        void StartPostPlacement(object sender, EventArguments e) {
            PlaceLightingObjects();
        }

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            GameObject parent = new GameObject();

            foreach (Edge edge in Edge.Edges) {
                var edgeMesh = Instantiate(OuterEdge, GetDefaultPositionVector(coordinate, true), edge.Rotation);
                edgeMesh.transform.parent = parent.transform;
            }
            parent.isStatic = true;
            parent.name = string.Format("Cube at {0}:{1}", coordinate.X, coordinate.Y);
        }

        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            var go = (bool) ChancesToSpawnFloors.GetRandom(typeof(bool));
            if (go) {
                Instantiate((GameObject) _biomeFloors.GetRandom(typeof(GameObject)),
                    GetDefaultPositionVector(coordinate, false), Quaternion.identity);
            }
        }

        private void PlaceLightingObjects() {
            var biomesCollection = GetTileCollectionForBiome(Biome.Earth);
            foreach (var biome in biomesCollection) {
                var walkableTiles = from tiles in biome.tiles where tiles.Type == Tile.Variant.Empty select tiles;
                foreach (var tile in walkableTiles) {
                    var particles = ParticleSystem.Instantiate(NightParticles, GetDefaultPositionVector(tile.Position, false), Quaternion.identity);
                    _particleList.Add(particles);
                }
            }
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
