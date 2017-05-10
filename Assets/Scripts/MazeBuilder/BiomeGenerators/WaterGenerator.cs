using App;
using App.EventSystem;
using MazeBuilder.Utility;
using PlayerLocationManager;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class WaterGenerator : AbstractBiomeGenerator {
        [Header("Rain Effect")]
        public GameObject Rain;

        private GameObject _instancedRain;


        private new void Awake() {
            base.Awake();
            Eventhub.Subscribe("maze:biomeChanged", ToggleBiomeWeather, this);
            InstantiateWeather();
        }

        private void InstantiateWeather() {
            _instancedRain = Instantiate(Rain, Vector3.back, Quaternion.identity);
            _instancedRain.GetComponent<EffectsNearPlayer>().StopEffect();
        }

        private void ToggleBiomeWeather(object sender, EventArguments e) {
            if (e.BiomeName == "Water Biome") {
                _instancedRain.GetComponent<EffectsNearPlayer>().StartEffect(e.Transform);
            } else {
                _instancedRain.GetComponent<EffectsNearPlayer>().StopEffect();
            }
        }

        public override void CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            Instantiate(FlatWall, Utils.GetDefaultPositionVector(coordinate), Quaternion.identity);
        }
    }
}