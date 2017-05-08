using UnityEngine;

namespace App {
    public class AppLocalStorage: MonoBehaviour {
        public static AppLocalStorage Instance { get; private set; }

        private void Start() {
            if (Instance == null) {
                Instance = this;
            }
        }

        public bool IsAuthed() {
            return PlayerPrefs.GetString("token") != "";
        }

        public void SetAuth(string sessionId) {
            PlayerPrefs.SetString("token", sessionId);
        }

        public void ResetAuth() {
            PlayerPrefs.DeleteKey("token");
        }
    }
}