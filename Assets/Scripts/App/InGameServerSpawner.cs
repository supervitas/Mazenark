using UnityEngine;
using UnityEngine.Networking;

namespace App {
    public class InGameServerSpawner : NetworkBehaviour {
        public static InGameServerSpawner Instance { get; private set; }
        public void  Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }

        public GameObject ServerSpawn(GameObject go, Vector3 position, Quaternion rotation) {
            var instantiated = Instantiate(go, position, rotation);
            NetworkServer.Spawn(instantiated);
            return instantiated;
        }

    }
}