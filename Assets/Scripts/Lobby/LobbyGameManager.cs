using System;
using App;
using UnityEngine;
using UnityEngine.Networking;
using Constants;

namespace Lobby {
    public class LobbyGameManager : MonoBehaviour {
        private byte _playersCount;

        public void SetPlayersCount(byte players) {
            _playersCount = players;
        }

        public void PlayerLefted() {
            _playersCount--;
            NetworkHttpManager.Instance.PlayerLeftFromRoom();
            if (GameShouldFinish()) {
                // send game result.
                GetComponent<LobbyManager>().StopServer();
                var networkManager = NetworkHttpManager.Instance;
//                var result = networkManager.CreateRequest(NetworkConstants.GameResultUrl);

                GetComponent<LobbyManager>().StartServer();
            }
        }

        private bool GameShouldFinish() {
            return _playersCount == 0;
        }
    }
}