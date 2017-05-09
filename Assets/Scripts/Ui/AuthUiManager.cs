using App;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class AuthUiManager: MonoBehaviour {
        public static AuthUiManager Instance { get; private set; }
        [SerializeField] public GameObject AuthPanel;
        [SerializeField] public GameObject UserDataPanel;
        [SerializeField] public GameObject PlayerName;
        [SerializeField] public GameObject PlayerScore;
        [SerializeField] public GameObject PlayOnline;

        private Button _playButton;
        private Text _playerName;
        private Text _playerScore;

        private void Start() {
            if (Instance == null) {
                Instance = this;
            }
            _playButton = PlayOnline.GetComponent<Button>();
            _playerName = PlayerName.GetComponent<Text>();
            _playerScore = PlayerScore.GetComponent<Text>();
        }

        public void ToggleAuthPannel(bool authed) {
            AuthPanel.SetActive(!authed);
            _playButton.interactable = authed;
            if (authed) {
                SetPlayerData();
                UserDataPanel.SetActive(true);
            } else {
                UserDataPanel.SetActive(false);
            }
        }

        private void SetPlayerData() {
            var user = AppLocalStorage.Instance.GetUserData();
            _playerName.text = user.username;
            _playerScore.text = string.Format("Score: {0}", user.score);
        }

    }
}