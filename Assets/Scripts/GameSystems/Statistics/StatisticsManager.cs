using System.Collections.Generic;
using App.Eventhub;
using Constants;
using UnityEngine;

namespace GameSystems.Statistics {
    public class StatisticsManager {
        private List<User> _users;
        public StatisticsManager(List<User> playersInfo) {
            _users = playersInfo;
        }
        
        public void PlayerDied() {
            
        }

        public void PlayerKilledEnemy() {
            
        }

        public void PlayerKilledPlayer() {
            
        }
        public void PlayerCompletedLevel(object sender, EventArguments eventArguments) {
//            foreach (var player in FindObjectsOfType<LobbyPlayer>()) {
//                if (player.playerName == eventArguments.Message) {
//                    Debug.Log(player.playerName);
//                }
//            }
        }   
    }
}