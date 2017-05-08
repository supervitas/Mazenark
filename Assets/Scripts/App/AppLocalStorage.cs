using Constants;
using UnityEngine;

namespace App {
    public class AppLocalStorage: MonoBehaviour {
        public static AppLocalStorage Instance { get; private set; }

        private void Start() {
            if (Instance == null) {
                Instance = this;
            }
        }

        public string GetToken() {
            return PlayerPrefs.GetString("token");
        }

        public bool IsAuthed() {
            return PlayerPrefs.GetString("token") != "";
        }

        public void SetUserData(User userData) {
            PlayerPrefs.SetString("token", userData.token);
            PlayerPrefs.SetString("username", userData.username);
            PlayerPrefs.SetInt("score", 0);
        }

        public User GetUserData() {
            return new User {username = PlayerPrefs.GetString("username"), score = 0};
        }

        public void ResetAuth() {
            PlayerPrefs.DeleteKey("token");
        }
    }
}