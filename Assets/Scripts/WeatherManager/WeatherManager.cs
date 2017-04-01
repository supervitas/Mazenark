using App.EventSystem;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;

namespace WeatherManager {
    public class WeatherManager : MonoBehaviour {
        private Maze _maze;
        public Transform Target;
        private Biome _currentBiome;
        private Publisher _eventhub;

        private void Start() {
            if (!Target) return;
            _maze = App.AppManager.Instance.MazeInstance.Maze;
            _eventhub = App.AppManager.Instance.EventHub;
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