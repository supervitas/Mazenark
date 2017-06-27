using System.Collections;
using System.Collections.Generic;
using App;
using UnityEngine;
using Constants;

namespace Lobby {
    public class LobbyGameManager : MonoBehaviour {
        private int _playersCount;
        private readonly List<Transform> _playersTransforms = new List <Transform>();

        public void AddPlayerTransform(Transform playerTransform) {
            _playersTransforms.Add(playerTransform);
        }

        public List<Transform> GetPlayersTransforms() {
            return _playersTransforms;
        }

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