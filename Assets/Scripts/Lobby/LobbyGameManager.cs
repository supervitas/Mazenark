using System.Collections;
using System.Collections.Generic;
using App;
using UnityEngine;
using Constants;

namespace Lobby {
    public class LobbyGameManager : MonoBehaviour {
        private byte _playersCount;
        public List<Transform> PlayersTransforms = new List <Transform>();

        public void OnGameover(string playerName) {
//            foreach (var x in LobbyManager.SSingleton.lobbySlots ) {
//                var player = x as LobbyPlayer;
//                if (player != null && player.playerName == playerName) {
////                    Destroy(x.);
////                    StartCoroutine(DestroyPlayer(player));
//                }
//            }
        }

        IEnumerator DestroyPlayer(LobbyPlayer player) {
            yield return new WaitForSeconds(2f);
            Destroy(player);
        }

        public void SetPlayersCount(byte players) {
            _playersCount = players;
        }

        public void PlayerLefted() {
            _playersCount--;
            if (!GameShouldFinish()) return;

            PlayersTransforms.Clear();

            GetComponent<LobbyManager>().StopServer();
            GetComponent<LobbyManager>().StartServer(); // restart server
            NetworkHttpManager.Instance.SendRoomUpdate(NetworkConstants.RoomGameEnded);
        }

        private bool GameShouldFinish() {
            return _playersCount == 0;
        }
    }
}