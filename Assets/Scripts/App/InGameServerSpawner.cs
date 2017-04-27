using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace App {
    public class InGameServerSpawner : NetworkBehaviour {
        public GameObject Safehouse;
        public static InGameServerSpawner Instance { get; private set; }
        public void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }

        private void Start() {
            if (isServer) {
                SpawnSafehouse();
            }
        }

        public GameObject ServerSpawn(GameObject go, Vector3 position, Quaternion rotation) {
            var instantiated = Instantiate(go, position, rotation);
            NetworkServer.Spawn(instantiated);
            return instantiated;
        }

        public void SpawnSafehouse() {
//            ServerSpawn(Safehouse, new Vector3(40, 0, 40), Quaternion.identity);
            ServerSpawn(Safehouse,
                Utils.GetDefaultPositionVector(new Coordinate(AppManager.Instance.MazeInstance.Height / 2,
                    AppManager.Instance.MazeInstance.Width / 2), 0.1f), Quaternion.identity);
        }

    }
}