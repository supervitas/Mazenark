using System;
using System.Collections.Generic;
using App;
using UnityEngine;
using UnityEngine.Networking;
using Constants;

namespace Lobby {
    public class LobbyGameManager : MonoBehaviour {
        private byte _playersCount;
        public Dictionary<string, Transform> PlayersTransforms = new Dictionary<string, Transform>();

        public void SetPlayersCount(byte players) {
            _playersCount = players;
        }

        public void PlayerLefted() {
            _playersCount--;
            if (!GameShouldFinish()) return;

            GetComponent<LobbyManager>().StopServer();
            NetworkHttpManager.Instance.SendRoomUpdate(NetworkConstants.RoomGameEnded);
            GetComponent<LobbyManager>().StartServer(); // restart server
        }

        private bool GameShouldFinish() {
            return _playersCount == 0;
        }
    }
}