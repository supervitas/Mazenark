using UnityEngine;

namespace App {
    public class AppLocalStorage: MonoBehaviour {
        public bool IsAuthed() {
            return PlayerPrefs.GetString("session") != "";
        }

        public void SetAuth(string sessionId) {
            PlayerPrefs.SetString("session", sessionId);
        }

        public void ResetAuth() {
            PlayerPrefs.DeleteKey("session");
        }
    }
}