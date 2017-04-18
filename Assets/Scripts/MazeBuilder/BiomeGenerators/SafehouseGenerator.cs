using App;
using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class SafehouseGenerator : AbstractBiomeGenerator {

        #region BiomeSafehouse
        [Header("Safehouse")]
        public GameObject Safehouse;
        #endregion

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
            PlaceSafeHouse();
        }

        private void PlaceLightingObjects() {
            ParticleList = PlaceLightingParticles(Biome.Safehouse, NightParticles);
        }

        private void PlaceSafeHouse() {
            InGameServerSpawner.Instance.ServerSpawn(Safehouse,
                Utils.GetDefaultPositionVector(new Coordinate(AppManager.Instance.MazeInstance.Height / 2,
                AppManager.Instance.MazeInstance.Width / 2), 0.1f), Quaternion.identity);
        }

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            AppManager.Instance.InstantiateSOC(FlatWall, Utils.GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
    }
}