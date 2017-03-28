using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class WaterGenerator : AbstractBiomeGenerator {
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
            ParticleList = PlaceLightingParticles(Biome.Water, NightParticles);
        }

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            Instantiate(FlatWall, GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            if (FloorSpawnChance >= UnityEngine.Random.Range(1, 101)) {
                Instantiate((GameObject) _biomeFloors.GetRandom(typeof(GameObject)),
                    GetDefaultPositionVector(coordinate, 0.1f), Quaternion.identity);
            }
        }
    }
}