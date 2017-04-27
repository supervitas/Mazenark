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
                // todo send results here
                GetComponent<LobbyManager>().StopServer();

                GetComponent<LobbyManager>().StartServer();
            }
        }

        public bool GameShouldFinish() {
            return _playersCount == 0;
        }
    }
}