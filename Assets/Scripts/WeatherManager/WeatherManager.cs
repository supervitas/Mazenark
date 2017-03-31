using MazeBuilder;
using UnityEngine;

namespace WeatherManager {
    public class WeatherManager : MonoBehaviour {
        private Maze _maze;
        public Transform Target;
        private Biome currentBiome;
        private void Awake() {
            _maze = App.AppManager.Instance.MazeInstance.Maze;
            InvokeRepeating("CheckBiomeChanged", 0, 5);
        }

        private void CheckBiomeChanged() {
//            Target.

        }


    }
}