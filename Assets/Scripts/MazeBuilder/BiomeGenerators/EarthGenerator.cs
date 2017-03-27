using System.Collections.Generic;
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

        private readonly CollectionRandom _biomeFloors = new CollectionRandom();

        private new void Awake() {
            base.Awake();
            _biomeFloors.Add(Floor, typeof(GameObject), 1.0f);
        }

        protected override void OnNight(object sender, EventArguments args) {
            EnableParticles();
        }

        protected override void OnDay(object sender, EventArguments args) {
            DisableParticles();
        }

        private void EnableParticles() {
            foreach (var particles in ParticleList) {
                particles.Play();
            }

        }
        private void DisableParticles() {
            foreach (var particles in ParticleList) {
                particles.Stop();
            }
        }

        protected override void StartPostPlacement(object sender, EventArguments e) {
            PlaceLightingObjects();
        }

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            GameObject parent = new GameObject();

            foreach (Edge edge in Edge.Edges) {
                var edgeMesh = Instantiate(OuterEdge, GetDefaultPositionVector(coordinate), edge.Rotation);
                edgeMesh.transform.parent = parent.transform;
            }
            parent.isStatic = true;
            parent.name = string.Format("Cube at {0}:{1}", coordinate.X, coordinate.Y);
        }

        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            var shouldPlace = (bool) SpawnObjectsChances["floor"].GetRandom(typeof(bool));
            if (shouldPlace) {
                Instantiate((GameObject) _biomeFloors.GetRandom(typeof(GameObject)),
                    GetDefaultPositionVector(coordinate, 0.1f), Quaternion.identity);
            }
        }

        private void PlaceLightingObjects() {
            ParticleList = PlaceLightingParticles(Biome.Earth, NightParticles);
            PlaceTorches();
        }

        private void PlaceTorches() {
            var mazeTiles = App.AppManager.Instance.MazeInstance.Maze;

            for (var i = 0; i < mazeTiles.Width; i++) {
                for (var j = 0; j < mazeTiles.Height - 1; j++) {
                    var wall = mazeTiles[i, j];
                    if (wall.Type == Tile.Variant.Wall && mazeTiles[i, j + 1].Type != Tile.Variant.Wall
                                                       && mazeTiles[i, j - 1].Type != Tile.Variant.Wall) {
                        PlaceTorch(wall);
                    }
                }
            }
        }

        private void PlaceTorch(Tile tile) {
            if (UnityEngine.Random.Range(0, 5) <= 2) return;
            var sideOffset = UnityEngine.Random.Range(0, 2) > 0 ? -0.55f : 0.55f;
            var rotation = sideOffset > 0 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, 270, 0);

            var position = new Vector3 {
                x = Utils.TransformToWorldCoordinate(tile.Position.X - Random.Range(-0.3f, 0.4f)),
                y = Constants.Maze.TILE_SIZE - 3,
                z = Utils.TransformToWorldCoordinate(tile.Position.Y - sideOffset)
            };

            Instantiate(Torch, position, rotation);
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
