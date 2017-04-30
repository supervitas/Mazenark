using System;
using App;
using Constants;
using NUnit.Framework.Internal;
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

        public void OnEnable() {
            lobbyManager.topPanel.ToggleVisibility(true);

            ipInput.onEndEdit.RemoveAllListeners();
            ipInput.onEndEdit.AddListener(onEndEditIP);

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

        internal class JsonPort {
            public int port;
        }
        public void OnClickPlayOnline() {
            Action<string> callback = result => { // inline callback which takes result http body as a param
                lobbyManager.ChangeTo(lobbyPanel);

                var jsonPort = JsonUtility.FromJson<JsonPort>(result);

                lobbyManager.networkAddress = NetworkConstants.GameRoomAdress;
                lobbyManager.networkPort = jsonPort.port;

                lobbyManager.StartClient();

                lobbyManager.backDelegate = lobbyManager.StopClientClbk;
                lobbyManager.DisplayIsConnecting();

                lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
            };
            NetworkHttpManager.Instance.GetRequest(NetworkConstants.GameGetRoom, callback);
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
