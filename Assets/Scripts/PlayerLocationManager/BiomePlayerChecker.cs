using App.Eventhub;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayerLocationManager {
    public class BiomePlayerChecker : NetworkBehaviour  {
        private Maze _maze;
        public Transform Target;
        private Biome _currentBiome;
        private Publisher _eventhub;

        public override void OnStartLocalPlayer() { // Set up game for client
            _eventhub = App.AppManager.Instance.EventHub;
            _maze = App.AppManager.Instance.MazeInstance.Maze;
            InvokeRepeating(nameof(CheckBiomeChanged), 0, 1.5f);
        }

        private void OnDestroy() {
            CancelInvoke(nameof(CheckBiomeChanged));
        }


        private void CheckBiomeChanged() {
            if (Target == null) return;

            var mazeCoords = Utils.TransformWorldToLocalCoordinate(Target.position.x, Target.position.z);
            var biome = _maze[mazeCoords.X, mazeCoords.Y].Biome;
            if (_currentBiome == biome) return;
            _currentBiome = biome;
            _eventhub.CreateEvent("maze:biomeChanged", new EventArguments(Target, _currentBiome.Name));
        }
    }
}