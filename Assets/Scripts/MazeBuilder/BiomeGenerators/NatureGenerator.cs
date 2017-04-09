using App;
using App.EventSystem;
using MazeBuilder.BiomeGenerators.PlacementRules;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class NatureGenerator : AbstractBiomeGenerator {
        // should be an array...
        [Header("Biome Placing Rules")]
        [SerializeField]
        private PlacementRule _outerEdges;
        [SerializeField]
        private PlacementRule _innerEdges;
        [SerializeField]
        private PlacementRule _straightWalls;

        [Header("Biome floors")]
        [SerializeField]
        private GameObject _floor2;


        private new void Awake() {
            base.Awake();
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
                var edgeMeshTemplate = _outerEdges.GetMeshForPlacement(maze, coordinate, edge);
                if (edgeMeshTemplate == null)
                    edgeMeshTemplate = _innerEdges.GetMeshForPlacement(maze, coordinate, edge);
                if (edgeMeshTemplate == null)
                    edgeMeshTemplate = _straightWalls.GetMeshForPlacement(maze, coordinate, edge);

                if (edgeMeshTemplate != null) {
                    var edgeMesh = AppManager.Instance.InstantiateSOC(edgeMeshTemplate, GetDefaultPositionVector(coordinate), edge.Rotation);
                    edgeMesh.name = string.Format(edge.Name);
                    edgeMesh.transform.parent = parent.transform;
                }
            }

            parent.name = string.Format("Cube at {0}:{1}", coordinate.X, coordinate.Y);
        }

        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            if (FloorEnviromentSpawnChance >= Random.Range(1, 100)) {
                Instantiate((GameObject) BiomeFloorsEnviroment.GetRandom(typeof(GameObject)),
                    GetDefaultPositionVector(coordinate, 0.2f), Quaternion.identity);
            }

            AppManager.Instance.InstantiateSOC(_floor2, GetDefaultPositionVector(coordinate), Edge.UpRight.Rotation);
        }

        private void PlaceLightingObjects() {
            ParticleList = PlaceLightingParticles(Biome.Nature, NightParticles);
        }

    }
}