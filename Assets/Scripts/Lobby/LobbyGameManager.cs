using System.Collections.Generic;
using App;
using App.Eventhub;
using UnityEngine;
using Constants;
using UnityEngine.Networking;

namespace Lobby {
    public class LobbyGameManager : NetworkBehaviour {
        private int _playersCount;
        private readonly List<Transform> _playersTransforms = new List <Transform>();

        private void Start() {
            if (!isServer) return;
            AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", PlayerCompletedMaze, this);
            AppManager.Instance.EventHub.Subscribe("PlayerDied", PlayerDied, this);            
        }

        private void PlayerCompletedMaze(object sender, EventArguments e) {
            if (!isServer) return;            
            Destroy(GameObject.Find(e.Message));
        }

        private void PlayerDied(object sender, EventArguments e) {
            if (!isServer) return;
            Destroy(GameObject.Find(e.Message));
        }


        public void AddPlayerTransform(Transform playerTransform) {
            _playersTransforms.Add(playerTransform);
        }

        public List<Transform> GetPlayersTransforms() {
            return _playersTransforms;
        }

        public void SetPlayersCount(int players) {
            _playersCount = players;
        }

        public void PlayerLefted() {
            _playersCount--;
            if (!GameShouldFinish()) return;

            _playersTransforms.Clear();

            GetComponent<LobbyManager>().StopServer();
            GetComponent<LobbyManager>().StartServer();
            NetworkHttpManager.Instance.SendRoomUpdate(NetworkConstants.RoomGameEnded);
        }

        private bool GameShouldFinish() {
            return _playersCount == 0;
        }
    }
}