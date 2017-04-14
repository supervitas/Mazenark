using App;
using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class WindGenerator : AbstractBiomeGenerator {

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
            ParticleList = PlaceLightingParticles(Biome.Wind, NightParticles);
            PlaceTorches(Biome.Wind);
        }

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            AppManager.Instance.InstantiateSOC(FlatWall, Utils.GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
    }
}