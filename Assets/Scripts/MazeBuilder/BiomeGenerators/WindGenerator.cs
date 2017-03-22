using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class WindGenerator : AbstractBiomeGenerator {
        #region BiomeWalls
        [Header("Biome Walls")]
        public GameObject FlatWall;
        #endregion

        #region BiomeFloor
        [Header("Biome Floor")]
        public GameObject Floor;
        #endregion

        #region BiomeFloor
        [Header("Biome Lighting Objetcs")]
        public ParticleSystem NightParticles;
        public GameObject Torch;
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

        private void PlaceLightingObjects() {
            ParticleList = PlaceLightingParticles(Biome.Wind, NightParticles);
        }

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
           Instantiate(FlatWall, GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            var shouldPlace = (bool) SpawnObjectsChances["floor"].GetRandom(typeof(bool));
            if (shouldPlace) {
                Instantiate((GameObject) _biomeFloors.GetRandom(typeof(GameObject)),
                    GetDefaultPositionVector(coordinate, 0.1f), Quaternion.identity);
            }
        }
    }
}