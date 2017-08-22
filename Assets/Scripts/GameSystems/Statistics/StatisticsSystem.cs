using System.Collections.Generic;
using Constants;

namespace GameSystems.Statistics {
    public class StatisticSystem {
        private readonly List<User> _users;
        
        
        public StatisticSystem(List<User> playersInfo) {
            _users = playersInfo;
        }                

        public void EnemyKilledPlayer(string playerName) {
            var player = _users.Find(user => user.username == playerName);
            if (player != null) {
                player.score -= 10;
                player.itemsInInventories = new ItemsInInventory[0];
            }
        }
        
        public void PlayerKilledEnemy(string playerName, int scoreToAdd = 2) {
            var player = _users.Find(user => user.username == playerName);
            if (player != null) {
                player.score += scoreToAdd;
            }
        }

        public void PlayerKilledPlayer(string playerKilled, string playerDead) {
            var whoKilled = _users.Find(user => user.username == playerKilled);
            var whoDied = _users.Find(user => user.username == playerDead);
            
            if (whoDied != null) {
                whoDied.score -= 25;
                whoDied.itemsInInventories = new ItemsInInventory[0];
            }
            
            if (whoKilled != null) {
                whoKilled.score += 25;
            }
        }
        
        public void PlayerCompletedLevel(string playerName) {
            var player = _users.Find(user => user.username == playerName);
            if (player != null) {
                player.score += 200;
            }
        }
                       
    }
}