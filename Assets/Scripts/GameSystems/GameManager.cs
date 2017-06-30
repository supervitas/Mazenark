using System.Collections.Generic;
using Constants;
using GameSystems.Statistics;
using UnityEngine;

namespace GameSystems {
    public class GameManager : MonoBehaviour {        
        private readonly List<User> _playersData = new List<User>();
        private readonly List<Transform> _playersTransforms = new List <Transform>();
        private StatisticsManager _statisticsManager;
        
        public void Start() {
//            AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", PlayerCompletedLevel, this);
            _statisticsManager = new StatisticsManager(_playersData);
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

        public void GameEnded() {
            _playersData.Clear();
            _playersTransforms.Clear();
            //send stats            
        }
                       
    }
}