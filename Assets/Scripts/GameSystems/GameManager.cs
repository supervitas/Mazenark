using System.Collections.Generic;
using App;
using App.Eventhub;
using Constants;
using GameSystems.Statistics;
using Lobby;
using UnityEngine;
using UnityEngine.Networking;

namespace GameSystems {
    public class GameManager : NetworkBehaviour {        
        private readonly List<User> _playersData = new List<User>();
        private readonly List<Transform> _playersTransforms = new List <Transform>();
        private StatisticsManager _statisticsManager;
        private int _playersCount;
        
        public void Start() {                         
            _statisticsManager = new StatisticsManager(_playersData);
        }
        
        
        public void PlayerCompletedMaze(GameObject player) {                     
            Destroy(player);
        }

        public void PlayerDied(GameObject player) {                        
            Destroy(player);
        }        
        
        public void AddPlayerTransform(Transform playerTransform) {
            _playersTransforms.Add(playerTransform);
        }

        public List<Transform> GetPlayersTransforms() {
            return _playersTransforms;
        }       

        public void AddPlayerData(User user) {
            _playersData.Add(user);
        }

        private void GameEnded() {
            _playersData.Clear();
            _playersTransforms.Clear();
            //send stats            
        }                             

        public void SetPlayersCount(int players) {
            _playersCount = players;
        }

        public void PlayerLefted() {
            _playersCount--;            
            if (_playersCount != 0) return;
            
            GameEnded();
                        
            if (AppManager.Instance.IsSinglePlayer) return;

            var lobbyManager = FindObjectOfType<LobbyManager>();
            lobbyManager.StopServer();
            lobbyManager.StartServer();
            NetworkHttpManager.Instance.SendRoomUpdate(NetworkConstants.RoomGameEnded);           
        }        
    }
                       
}