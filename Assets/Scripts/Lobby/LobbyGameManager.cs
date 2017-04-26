using UnityEngine;
using UnityEngine.Networking;

namespace Lobby {
    public class LobbyGameManager : MonoBehaviour {
        private byte _playersCount;

        public void SetPlayersCount(byte players) {
            _playersCount = players;
        }

        public void PlayerLefted() {
            _playersCount--;
            if (GameShouldFinish()) {
                GetComponent<LobbyManager>().StopServer();
            }
        }

        public bool GameShouldFinish() {
            return _playersCount == 0;
        }
    }
}