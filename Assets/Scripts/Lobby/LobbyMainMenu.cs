using System;
using App;
using Constants;
using Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby {
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour {
        public LobbyManager lobbyManager;

        public RectTransform lobbyServerList;
        public RectTransform lobbyPanel;

        public InputField ipInput;
        public InputField matchNameInput;
        public InputField loginInput;
        public InputField passwordInput;

        public LobbyInfoPanel infoPanel;

        public void OnEnable() {
            lobbyManager.topPanel.ToggleVisibility(true);

            ipInput.onEndEdit.RemoveAllListeners();
            ipInput.onEndEdit.AddListener(onEndEditIP);

        }

        public void OnClickRegisterAsGuest() {

            Action<string> callback = result => {
                var user = JsonUtility.FromJson<User>(result);
                AppLocalStorage.Instance.SetUserData(user);
                AuthUiManager.Instance.ToggleAuthPannel(true);
            };

            Action<string> errorCallback = error => {
                var errorJson = JsonUtility.FromJson<Error>(error);
                infoPanel.Display(errorJson.error, "Close", null);
            };

            NetworkHttpManager.Instance.RegisterAsGuest(callback, errorCallback);
        }

        public void OnClickRegister() {
            var login = loginInput.text;
            var password = passwordInput.text;
            if(login == "" || password == "") return;


            Action<string> callback = result => {
                var user = JsonUtility.FromJson<User>(result);
                AppLocalStorage.Instance.SetUserData(user);
                AuthUiManager.Instance.ToggleAuthPannel(true);
                login = "";
                password = "";
            };

            Action<string> errorCallback = error => {
                var errorJson = JsonUtility.FromJson<Error>(error);
                infoPanel.Display(errorJson.error, "Close", null);
            };

            NetworkHttpManager.Instance.AuthRequest(NetworkConstants.Register, new AuthData
                {password = password, username = login},  callback, errorCallback);
        }

        public void OnClickLogin() {
            var login = loginInput.text;
            var password = passwordInput.text;
            if(login == "" || password == "") return;

            Action<string> callback = result => {
                var user = JsonUtility.FromJson<User>(result);
                user.username = login;
                AppLocalStorage.Instance.SetUserData(user);
                AuthUiManager.Instance.ToggleAuthPannel(true);
                login = "";
                password = "";
            };

            Action<string> errorCb = error => {
                var errorJson = JsonUtility.FromJson<Error>(error);
                infoPanel.Display(errorJson.error, "Close", null);
            };

            NetworkHttpManager.Instance.AuthRequest(NetworkConstants.Login, new AuthData
                {password = password, username = login},  callback, errorCb);
        }

        public void OnClickLogout() {
            AppLocalStorage.Instance.ResetAuth();
            AuthUiManager.Instance.ToggleAuthPannel(false);

            NetworkHttpManager.Instance.Logout(new Token {token = AppLocalStorage.Instance.GetToken()});
        }

        public void OnClickSinglePlayer() {
            lobbyManager.StartSinglePlayer();
        }

        public void OnClickHost() {
            lobbyManager.StartHost();
        }

        public void OnClickJoin() {
            lobbyManager.ChangeTo(lobbyPanel);
            string[] addr = ipInput.text.Split(':');

            if (addr.Length > 1) {
                lobbyManager.networkPort = Convert.ToInt32(addr[1]);
            }
            lobbyManager.networkAddress = addr[0];

            lobbyManager.StartClient();

            lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
        }

        public void OnClickDedicated() {
            lobbyManager.ChangeTo(null);
            lobbyManager.StartServer();

            lobbyManager.backDelegate = lobbyManager.StopServerClbk;

            lobbyManager.SetServerInfo("Dedicated Server", lobbyManager.networkAddress);
        }

        public void OnClickCreateMatchmakingGame() {
            lobbyManager.StartMatchMaker();
            lobbyManager.matchMaker.CreateMatch(
                matchNameInput.text,
                (uint)lobbyManager.maxPlayers,
                true,
				"", "", "", 0, 0,
				lobbyManager.OnMatchCreate);

            lobbyManager.backDelegate = lobbyManager.StopHost;
            lobbyManager._isMatchmaking = true;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Matchmaker Host", lobbyManager.matchHost);
        }

        public void OnClickPlayOnline() {
            Action<string> callback = result => { // callback which takes result http body as a param
                lobbyManager.ChangeTo(lobbyPanel);

                var jsonPort = JsonUtility.FromJson<JsonPort>(result);

                lobbyManager.networkAddress = NetworkConstants.GameRoomAdress;
                lobbyManager.networkPort = jsonPort.port;

                lobbyManager.StartClient();

                lobbyManager.backDelegate = lobbyManager.StopClientClbk;
                lobbyManager.DisplayIsConnecting();

                lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
            };

            Action<string> errorCb = error => { // callback which takes result http body as a param
                var errorJson = JsonUtility.FromJson<Error>(error);
                infoPanel.Display(errorJson.error, "Close", null);
            };
            NetworkHttpManager.Instance.GetRequest(NetworkConstants.RoomGetRoom, callback, errorCb);
        }

        void onEndEditIP(string text) {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickJoin();
            }
        }

        void onEndEditGameName(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickCreateMatchmakingGame();
            }
        }

    }
}
