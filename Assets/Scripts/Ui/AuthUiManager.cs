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

        private string GetUserRang(int score) {
            if (score < 100) {
                return "Beginner";
            }
            if (score < 150) {
                return "Bronze Explorer";
            }
            if (score < 300) {
                return "Silver Explorer";
            }
            if (score < 500) {
                return "Golden Explorer";               
            }
            if (score < 750) {
                return "Master";
            }
            if (score < 1000) {
                return "Grandmaster";
            }

            return "Beginner";
        }

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
            _playerScore.text = $"Rang: {GetUserRang(user.score)}";
        }

    }
}