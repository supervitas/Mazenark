using System.Collections.Generic;
using GameSystems.Player;
using UnityEngine;

namespace GameSystems {
    public class GameManager : MonoBehaviour {        
        private readonly List<PlayerInfo> _playersData = new List<PlayerInfo>();
        private readonly List<Transform> _playersTransforms = new List <Transform>(); 
        
        
        public void Start() {
//            AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", PlayerCompletedLevel, this);
        }
        
        public void AddPlayerTransform(Transform playerTransform) {
            _playersTransforms.Add(playerTransform);
        }

        public List<Transform> GetPlayersTransforms() {
            return _playersTransforms;
        }       

        public void AddPlayerData(PlayerInfo playerInfo) {
            _playersData.Add(playerInfo);
        }
                       
    }
}