using App.EventSystem;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;

namespace WeatherManager {
    public class WeahterManagerLocal : MonoBehaviour {

            private Maze _maze;
            public Transform Target;
            private Biome _currentBiome;
            private Publisher _eventhub;

            public void Start() { // Set up game for client
                _eventhub = App.AppManager.Instance.EventHub;
                _maze = App.AppManager.Instance.MazeInstance.Maze;
                InvokeRepeating("CheckBiomeChanged", 0, 2);
            }

            private void OnDestroy() {
                CancelInvoke("CheckBiomeChanged");
            }

            private void CheckBiomeChanged() {
                if (Target == null) return;

                var mazeCoords = Utils.TransformWorldToLocalCoordinate(Target.position.x, Target.position.z);
                var biome = _maze[mazeCoords.X, mazeCoords.Y].Biome;
                if (_currentBiome == biome) return;
                _currentBiome = biome;
                _eventhub.CreateEvent("WeatherShouldChange", new EventArguments(Target, _currentBiome.Name));
            }
        }

}
