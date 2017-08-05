using System.Collections.Generic;
using App;
using Constants;
using GameSystems.Statistics;
using Lobby;
using UnityEngine;
using UnityEngine.Networking;

namespace GameSystems {
    public class GameManager : NetworkBehaviour {        
        private readonly List<User> _playersData = new List<User>();
        private static readonly List<Transform> _playersTransforms = new List <Transform>();
        
        
        private StatisticSystem _statisticsManager;
        private int _playersCount;
        
        public override void OnStartServer() {
            _playersTransforms.Clear();
            _statisticsManager = new StatisticSystem(_playersData);         
        }
        
        public void PlayerCompletedMaze(GameObject player) {
            _playersTransforms.Remove(player.transform);
            _statisticsManager.PlayerCompletedLevel(player.name);
            Destroy(player);
        }

        public void PlayerDied(GameObject player) {
            _playersTransforms.Remove(player.transform);
            _statisticsManager.PlayerDied(player.name);
            Destroy(player, 2f);            
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

        public void SetPlayersCount(int players) {
            _playersCount = players;
        }
        
        public void PlayerLefted() {
            _playersCount--;
            
            for (var i = 0; i < _playersTransforms.Count; i++) {
                if (_playersTransforms[i] == null) {
                    Debug.Log(_playersTransforms[i]);
                    _playersTransforms.RemoveAt(i);
                }
            }            
            
            if (_playersCount != 0) return;
            
            GameEnded();
                        
            if (AppManager.Instance.IsSinglePlayer) return;

            var lobbyManager = FindObjectOfType<LobbyManager>();
            lobbyManager.StopServer();
            lobbyManager.StartServer();
            NetworkHttpManager.Instance.SendRoomUpdate(NetworkConstants.RoomGameEnded);           
        }
        
        private void GameEnded() {
            _playersData.Clear();
            _playersTransforms.Clear();
            SendStatistics();
        }

        private void SendStatistics() {
            
        }
        
    }
                       
}