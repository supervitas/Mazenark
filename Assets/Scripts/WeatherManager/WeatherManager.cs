using App.EventSystem;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace WeatherManager {
    public class WeatherManager : NetworkBehaviour  {
        private Maze _maze;
        public Transform Target;
        private Biome _currentBiome;
        private Publisher _eventhub;

        public override void OnStartLocalPlayer() { // Set up game for client
            _eventhub = App.AppManager.Instance.EventHub;
            _eventhub.Subscribe("MazeLoaded", StartManager, this);
        }

        void StartManager(object sender, EventArguments eventArguments) {
            _maze = App.AppManager.Instance.MazeInstance.Maze;
            InvokeRepeating("CheckBiomeChanged", 0, 2);
        }

        private void CheckBiomeChanged() {
            var mazeCoords = Utils.TransformWorldToLocalCoordinate(Target.position.x, Target.position.z);
            var biome = _maze[mazeCoords.X, mazeCoords.Y].Biome;
            if (_currentBiome == biome) return;
            _currentBiome = biome;
            _eventhub.CreateEvent("WeatherShouldChange", new EventArguments(Target, _currentBiome.Name));
        }
    }
}