using System;
using Constants;
using Ui;
using UnityEngine;

namespace App {
    public class AppLocalStorage: MonoBehaviour {
        public static AppLocalStorage Instance { get; private set; }
        public User user;

        private void Start() {
            if (Instance == null) {
                Instance = this;
            }
            LoadUserData();
        }

        public string GetToken() {
            if (user == null) {
                LoadUserData();
            }
            return user.token;
        }

        public bool IsAuthed() {
            if (user == null) {
                LoadUserData();
            }
            return user.token != "";
        }

        public void SetUserData(User userData) {
            user = userData;
            PlayerPrefs.SetInt("id", userData.id);
            PlayerPrefs.SetString("isGuest", userData.isGuest.ToString());
            PlayerPrefs.SetString("token", userData.token);
            PlayerPrefs.SetString("username", userData.username);
            PlayerPrefs.SetInt("score", 0);
        }

        public User GetUserData() {
            if (user == null) {
                LoadUserData();
            }
            return user;
        }

        public void ResetAuth() {
            user = null;
            AuthUiManager.Instance.ToggleAuthPannel(false);
            PlayerPrefs.DeleteKey("token");
        }

        private void LoadUserData() {
            if (user == null) {
                user = new User {
                    id = PlayerPrefs.GetInt("id"),
                    isGuest = PlayerPrefs.GetString("isGuest") != "" && Convert.ToBoolean(PlayerPrefs.GetString("isGuest")),
                    score = PlayerPrefs.GetInt("score"),
                    token = PlayerPrefs.GetString("token"),
                    username = PlayerPrefs.GetString("username")
                };
            }
        }
    }
}