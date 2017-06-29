using App;
using App.Eventhub;
using UnityEngine;
using Constants;
using UnityEngine.Networking;

namespace Lobby {
    public class LobbyGameManager : NetworkBehaviour {
        private int _playersCount;      

        private void Start() {            
            AppManager.Instance.EventHub.Subscribe("maze:levelCompleted", PlayerCompletedMaze, this);
            AppManager.Instance.EventHub.Subscribe("PlayerDied", PlayerDied, this);            
        }       
        
        private void PlayerCompletedMaze(object sender, EventArguments e) {                     
            Destroy(GameObject.Find(e.Message));
        }

        private void PlayerDied(object sender, EventArguments e) {            
            Destroy(GameObject.Find(e.Message));
        }
        
        private bool GameShouldFinish() {
            return _playersCount == 0;
        }    

        public void SetPlayersCount(int players) {
            _playersCount = players;
        }

        public void PlayerLefted() {
            _playersCount--;
            if (!GameShouldFinish()) return;
            // Send Results
            GetComponent<LobbyManager>().StopServer();
            GetComponent<LobbyManager>().StartServer();
            NetworkHttpManager.Instance.SendRoomUpdate(NetworkConstants.RoomGameEnded);
        }        
    }
}