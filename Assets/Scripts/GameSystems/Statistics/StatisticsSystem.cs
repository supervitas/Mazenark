using System.Collections.Generic;
using Constants;

namespace GameSystems.Statistics {
    public class StatisticSystem {
        private readonly List<User> _users;
        
        
        public StatisticSystem(List<User> playersInfo) {
            _users = playersInfo;
        }
        
        public void PlayerDied(string playerName) {                       
            var player = _users.Find(user => user.username == playerName);
            if (player != null) {
                player.score -= 20;
            }            

        }

        public void PlayerKilledEnemy() {
            
        }

        public void PlayerKilledPlayer(string player) {
            
        }
        
        public void PlayerCompletedLevel(string playerName) {
            var player = _users.Find(user => user.username == playerName);
            if (player != null) {
                player.score += 200;
            }
        }
                       
    }
}