using App;
using App.EventSystem;
using MazeBuilder.Utility;
using Prefabs.Particles;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class WaterGenerator : AbstractBiomeGenerator {
        [Header("Rain Effect")]
        public GameObject Rain;

        private GameObject _instancedRain;


        private new void Awake() {
            base.Awake();
            Eventhub.Subscribe("WeatherShouldChange", ToggleBiomeWeather, this);
            InstantiateWeather();
        }

        private void InstantiateWeather() {
            _instancedRain = Instantiate(Rain, Vector3.back, Quaternion.identity);
            _instancedRain.GetComponent<RainFolowingPlayer>().StopRain();
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

        private void ToggleBiomeWeather(object sender, EventArguments e) {
            if (e.BiomeName == "Water Biome") {
                _instancedRain.GetComponent<RainFolowingPlayer>().StartRain(e.Transform);
            } else {
                _instancedRain.GetComponent<RainFolowingPlayer>().StopRain();
            }
        }


        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            AppManager.Instance.InstantiateSOC(FlatWall, GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
        public override void CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
//            if (FloorEnviromentSpawnChance >= UnityEngine.Random.Range(1, 100)) {
//                AppManager.Instance.InstantiateSOC((GameObject) BiomeFloorsEnviroment.GetRandom(typeof(GameObject)),
//                    GetDefaultPositionVector(coordinate, 0.1f), Quaternion.identity);
//            }
        }
    }
}