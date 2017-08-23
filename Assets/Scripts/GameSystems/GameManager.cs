using System.Collections.Generic;
using System.Linq;
using App;
using App.Eventhub;
using CharacterControllers;
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

        public void PlayerCompletedLevel(string playerName) {                       
            var player = GameObject.Find(playerName);            
            var playerController = player.GetComponent<ServerPlayerController>();
            
            var playerItems = playerController.GetPlayerItems();
            
            var playerData = _playersData.Find(user => user.username == playerName);           
            
            if (playerData != null) {
                playerData.itemsInInventory = new ItemsInInventory[playerItems.Count];
                var itemList = playerItems.ToArray();
                
                for (var i = 0; i < playerItems.Count; i++) {                   
                    playerData.itemsInInventory[i] = new ItemsInInventory{id = itemList[i].Key, 
                        chargesLeft = itemList[i].Value.ToString()};
                }
                playerData.score += 100;                
                NetworkHttpManager.Instance.UpdateUser(playerData);
                _playersData.Remove(playerData);
            }
            _playersTransforms.Remove(player.transform);            
            Destroy(player);
        }

        #region Statistics
              
            public void PlayerKilledEnemy(string playerName) {
                _statisticsManager.PlayerKilledEnemy(playerName);
            }
            
            public void EnemyKilledPlayer(string playerName) {
                _statisticsManager.EnemyKilledPlayer(playerName);
            }
             
            public void PlayerKilledPlayer(string playerKilled, string playerDied) {
                _statisticsManager.PlayerKilledPlayer(playerKilled, playerDied);
            }
    
            public void PlayerKilledBoss(string playerName) {
                _statisticsManager.PlayerKilledEnemy(playerName, 20);
            }                   
    
            public void PlayerDied(GameObject player) {
                _playersTransforms.Remove(player.transform);
                Destroy(player, 2f);
            }
                    
        #endregion

        #region GameObserver
           
            public void AddPlayerTransform(Transform playerTransform) {
                _playersTransforms.Add(playerTransform);
            }
    
            public List<Transform> GetPlayersTransforms() {
                return _playersTransforms;
            }

            public void AddPlayerData(User user) {            
                _playersData.Add(user);
            }

            public User GetPlayerData(string playerName) {
                return _playersData.FirstOrDefault(player => player.username == playerName);
            }

            public void SetPlayersCount(int players) {
                _playersCount = players;
            }

            public void PlayerLefted() {
                _playersCount--;

                for (var i = 0; i < _playersTransforms.Count; i++) {
                    if (_playersTransforms[i] == null) {                        
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
        
        #endregion               
        
        private void GameEnded() {
            Debug.Log(123);
            _playersData.Clear();
            _playersTransforms.Clear();
            SendStatistics();
        }

        private void SendStatistics() {
            
        }
        
    }
                       
}