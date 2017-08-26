using App;
using GameEnv.Teleports;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace GameEnv {
    public class GameEnvironmentManager : NetworkBehaviour {
        public GameObject Safehouse;        

        private void Start() {
            SpawnSafehouse();
            CreateTeleports();
        }

        public GameObject ServerSpawn(GameObject go, Vector3 position, Quaternion rotation) {
            var instantiated = Instantiate(go, position, rotation);
            NetworkServer.Spawn(instantiated);
            return instantiated;
        }

        private void SpawnSafehouse() {
//            ServerSpawn(Safehouse, new Vector3(40, 0, 40), Quaternion.identity);
            ServerSpawn(Safehouse,
                Utils.GetDefaultPositionVector(new Coordinate(AppManager.Instance.MazeInstance.Height / 2,
                    AppManager.Instance.MazeInstance.Width / 2), 0.1f), Quaternion.identity);
        }

        private void CreateTeleports() {
            GetComponent<TeleportManager>().CreateTeleports(4);
        }

    }
}