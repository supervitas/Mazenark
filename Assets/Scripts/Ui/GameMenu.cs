using Lobby;
using UnityEngine;

namespace Ui {
    public class GameMenu : MonoBehaviour {
        private LobbyTopPanel _lobbyTopPanel;
        private bool _wasPressed = false;

        private void Start() {
            _lobbyTopPanel = FindObjectOfType<LobbyTopPanel>();
        }

        public void ShowMenu() {
            _wasPressed = !_wasPressed;
            _lobbyTopPanel.ToggleVisibility(_wasPressed);               
        }
                
    }
}