using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace App {
    public class ServerSpawner : NetworkBehaviour {
        public GameObject Safehouse;
        public GameObject Fog;
        public static ServerSpawner Instance { get; private set; }
        public void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }

        private void Start() {
            SpawnSafehouse();
            SpawnFog();
        }

        public GameObject ServerSpawn(GameObject go, Vector3 position, Quaternion rotation) {
            var instantiated = Instantiate(go, position, rotation);
            NetworkServer.Spawn(instantiated);
            return instantiated;
        }

        private void SpawnFog() {
            ServerSpawn(Fog,
                Utils.GetDefaultPositionVector(new Coordinate(AppManager.Instance.MazeInstance.Height / 2,
                    AppManager.Instance.MazeInstance.Width / 2), 0.1f), Quaternion.Euler(-90, 0, 0));
        }
        private void SpawnSafehouse() {
//            ServerSpawn(Safehouse, new Vector3(40, 0, 40), Quaternion.identity);
            ServerSpawn(Safehouse,
                Utils.GetDefaultPositionVector(new Coordinate(AppManager.Instance.MazeInstance.Height / 2,
                    AppManager.Instance.MazeInstance.Width / 2), 0.1f), Quaternion.identity);
        }

    }
}