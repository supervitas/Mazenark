using System;
using System.Collections.Generic;
using App;
using UnityEngine;
using UnityEngine.Networking;
using Constants;

namespace Lobby {
    public class LobbyGameManager : MonoBehaviour {
        private byte _playersCount;
        public List<Transform> PlayersTransforms = new List <Transform>();

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