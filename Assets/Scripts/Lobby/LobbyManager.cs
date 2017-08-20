using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App;
using Constants;
using Controls;
using GameSystems;
using MazeBuilder.Utility;
using Ui;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lobby{
    public class LobbyManager : NetworkLobbyManager {
        static short MsgKicked = MsgType.Highest + 1;

        public static LobbyManager SSingleton;


        [Header("Unity UI Lobby")]
        [Tooltip("Time in second between all players ready & match start")]
        public float prematchCountdown = 5.0f;

        [Space]
        [Header("UI Reference")]
        public LobbyTopPanel topPanel;

        public RectTransform mainMenuPanel;
        public RectTransform lobbyPanel;

        public LobbyInfoPanel infoPanel;
        public LobbyCountdownPanel countdownPanel;
        public GameObject addPlayerButton;

        protected RectTransform currentPanel;

        public Button backButton;

        public Text statusInfo;
        public Text hostInfo;


        //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
        //of players, so that even client know how many player there is.
        [HideInInspector]
        public int _playerNumber = 0;

        //used to disconnect a client properly when exiting the matchmaker
        [HideInInspector]
        public bool _isMatchmaking = false;

        protected bool _disconnectServer = false;
        
        protected ulong _currentMatchID;

        protected LobbyHook _lobbyHooks;

        private IEnumerator<Vector3> _spawnGenerator;

        [HideInInspector] public int InstanceId;

        private AppLocalStorage _storage;

        private Dictionary<string, string> _playersTokens = new Dictionary<string, string>();


        void Start() {
            SSingleton = this;
            _lobbyHooks = GetComponent<LobbyHook>();
            currentPanel = mainMenuPanel;

            backButton.gameObject.SetActive(false);
            GetComponent<Canvas>().enabled = true;

            DontDestroyOnLoad(gameObject);

            _storage = FindObjectOfType<AppLocalStorage>();

            AuthUiManager.Instance.ToggleAuthPannel(_storage.IsAuthed());

            SetServerInfo("Offline", "None");
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn) {
            if (SceneManager.GetSceneAt(0).name == lobbyScene) {
                if (topPanel.isInGame) {
                    ChangeTo(lobbyPanel);
                    if (_isMatchmaking) {
                        if (conn.playerControllers[0].unetView.isServer) {
                            backDelegate = StopHostClbk;
                        } else {
                            backDelegate = StopClientClbk;
                        }
                    } else {
                        if (conn.playerControllers[0].unetView.isClient) {
                            backDelegate = StopHostClbk;
                        } else {
                            backDelegate = StopClientClbk;
                        }
                    }
                }
                else {
                    ChangeTo(mainMenuPanel);
                }

                topPanel.ToggleVisibility(true);
                topPanel.isInGame = false;
            } else {
                ChangeTo(null);

                Destroy(GameObject.Find("MainMenuUI(Clone)"));

                //backDelegate = StopGameClbk;
                topPanel.isInGame = true;
                topPanel.ToggleVisibility(false);
            }
        }

        public void ChangeTo(RectTransform newPanel) {
            if (currentPanel != null) {
                currentPanel.gameObject.SetActive(false);
            }

            if (newPanel != null) {
                newPanel.gameObject.SetActive(true);
            }

            currentPanel = newPanel;

            if (currentPanel != mainMenuPanel) {
                backButton.gameObject.SetActive(true);
            } else {
                backButton.gameObject.SetActive(false);
                SetServerInfo("Offline", "None");
                _isMatchmaking = false;
            }
        }

        public void DisplayIsConnecting() {
            var _this = this;
            infoPanel.Display("Connecting...", "Cancel", () => { _this.backDelegate(); });
        }

        public void SetServerInfo(string status, string host) {
            statusInfo.text = status;
            hostInfo.text = host;
        }


        public delegate void BackButtonDelegate();
        public BackButtonDelegate backDelegate;
        public void GoBackButton() {
            backDelegate();
			topPanel.isInGame = false;
        }

        // ----------------- Server management

        public void AddLocalPlayer() {
            TryToAddPlayer();
        }

        public void RemovePlayer(LobbyPlayer player) {
            player.RemovePlayer();
        }

        public void SimpleBackClbk() {
            ChangeTo(mainMenuPanel);
        }
                 
        public void StopHostClbk() {
            if (_isMatchmaking) {
				matchMaker.DestroyMatch((NetworkID)_currentMatchID, 0, OnDestroyMatch);
				_disconnectServer = true;
            }
            else {
                StopHost();
            }

            
            ChangeTo(mainMenuPanel);
        }

        public void StopClientClbk() {
            StopClient();

            if (_isMatchmaking) {
                StopMatchMaker();
            }

            ChangeTo(mainMenuPanel);
        }

        public void StopServerClbk() {
            StopServer();
            ChangeTo(mainMenuPanel);
        }

        class KickMsg : MessageBase { }
        public void KickPlayer(NetworkConnection conn) {
            conn.Send(MsgKicked, new KickMsg());
        }


        public void KickedMessageHandler(NetworkMessage netMsg) {
            infoPanel.Display("Kicked by Server", "Close", null);
            netMsg.conn.Disconnect();
        }

        //===================

        public void StartSinglePlayer() {
            minPlayers = 1;
            prematchCountdown = 0;
            StartHost();
        }

        public override void OnStartHost() {
            base.OnStartHost();
            ChangeTo(lobbyPanel);
            backDelegate = StopHostClbk;
            SetServerInfo("Hosting", networkAddress);         
        }

        public void StartDedicatedServerInstance(int port, int instanceId) {
            InstanceId = instanceId;
            networkPort = port;
            StartServer();
            Debug.Log($"instance started  {networkAddress}:{networkPort}");
            SetServerInfo("Dedicated Server", networkAddress);
        }

        public override void OnStartServer() {
            base.OnStartServer();

            AppManager.Instance.Init();
            AppManager.Instance.MazeSize.GenerateRandomSize();
            AppManager.Instance.MazeInstance = new MazeBuilder.MazeBuilder(AppManager.Instance.MazeSize.X, AppManager.Instance.MazeSize.Y);

            _spawnGenerator = GetSpawnPosition();
            _spawnGenerator.MoveNext();
        }

		public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
			base.OnMatchCreate(success, extendedInfo, matchInfo);
            _currentMatchID = (System.UInt64)matchInfo.networkId;

		}


		public override void OnDestroyMatch(bool success, string extendedInfo) {
			base.OnDestroyMatch(success, extendedInfo);
			if (_disconnectServer) {
                StopMatchMaker();
                StopHost();
            }
        }

        //allow to handle the (+) button to add/remove player
        public void OnPlayersNumberModified(int count) {
            _playerNumber += count;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += p == null || p.playerControllerId == -1 ? 0 : 1;

            addPlayerButton.SetActive(localPlayerCount < maxPlayersPerConnection && _playerNumber < maxPlayers);
        }


        // ----------------- Server callbacks ------------------

        //we want to disable the button JOIN if we don't have enough player
        //But OnLobbyClientConnect isn't called on hosting player. So we override the lobbyPlayer creation
        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId) {
            NetworkHttpManager.Instance.SendRoomUpdate(NetworkConstants.RoomPlayerJoined);

            var obj = Instantiate(lobbyPlayerPrefab.gameObject);

            LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();

            newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);

            foreach (NetworkLobbyPlayer t in lobbySlots) {
                var p = t as LobbyPlayer;

                if (p == null) continue;
                p.RpcUpdateRemoveButton();
                p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
            }

            return obj;
        }


        public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId) {
            foreach (NetworkLobbyPlayer t in lobbySlots) {
                var p = t as LobbyPlayer;

                if (p == null) continue;
                p.RpcUpdateRemoveButton();
                p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
            }
        }

        public override void OnLobbyServerDisconnect(NetworkConnection conn) {
            NetworkHttpManager.Instance.SendRoomUpdate(NetworkConstants.RoomPlayerLeft);            
            FindObjectOfType<GameManager>().PlayerLefted();

            foreach (var t in lobbySlots) {
                LobbyPlayer p = t as LobbyPlayer;

                if (p == null) continue;
                p.RpcUpdateRemoveButton();
                p.ToggleJoinButton(numPlayers >= minPlayers);
            }
        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
            //This hook allows you to apply state data from the lobby-player to the game-player
            //just subclass "LobbyHook" and add it to the lobby object.
            if (_lobbyHooks)
                _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);
            
            var nickName = lobbyPlayer.GetComponent<LobbyPlayer>().playerName;
            if (nickName == "") {
                nickName = "You";
            }

            gamePlayer.name = nickName;
            gamePlayer.GetComponent<PlayerControl>().SetPlayerName(gamePlayer.name);
            gamePlayer.transform.position = _spawnGenerator.Current;
            _spawnGenerator.MoveNext();
            
            var gameManager = FindObjectOfType<GameManager>();           
            gameManager.AddPlayerData(new User{username = gamePlayer.name});

            return true;
        }


        private IEnumerator<Vector3> GetSpawnPosition() {
            var spawns = AppManager.Instance.MazeInstance.Maze.Spawns;
            foreach (var spawn in spawns) {
                yield return new Vector3 {
                    x = Utils.TransformToWorldCoordinate(spawn.Center.X),
                    y = 0.1f,
                    z = Utils.TransformToWorldCoordinate(spawn.Center.Y)
                };
            }
        }
        // --- Countdown management

        public override void OnLobbyServerPlayersReady() {
			var allready = true;
			foreach (NetworkLobbyPlayer t in lobbySlots) {
			    if(t != null)
			        allready &= t.readyToBegin;
			}
            if (allready) {
                NetworkHttpManager.Instance.SendRoomUpdate(NetworkConstants.RoomGameStarted);
                FindObjectOfType<GameManager>().SetPlayersCount(lobbySlots.Count(player => player != null));
                StartCoroutine(ServerCountdownCoroutine());
            }
        }

        public IEnumerator ServerCountdownCoroutine() {
            float remainingTime = prematchCountdown;
            int floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0) {
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime) {//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                    floorTime = newFloorTime;

                    foreach (NetworkLobbyPlayer t in lobbySlots) {
                        if (t != null)
                        {//there is maxPlayer slots, so some could be == null, need to test it before accessing!
                            (t as LobbyPlayer).RpcUpdateCountdown(floorTime);
                        }
                    }
                }
            }

            foreach (NetworkLobbyPlayer t in lobbySlots) {
                if (t == null) continue;
                var lobbyPlayer = t as LobbyPlayer;
                if (lobbyPlayer != null) lobbyPlayer.RpcUpdateCountdown(0);
            }

            ServerChangeScene(playScene);
        }

        // ----------------- Client callbacks ------------------

        public override void OnClientConnect(NetworkConnection conn) {
            base.OnClientConnect(conn);

            infoPanel.gameObject.SetActive(false);

            conn.RegisterHandler(MsgKicked, KickedMessageHandler);

            if (!NetworkServer.active) {//only to do on pure client (not self hosting client)
                ChangeTo(lobbyPanel);
                backDelegate = StopClientClbk;
                SetServerInfo("Client", networkAddress);
            }
        }



        public override void OnClientDisconnect(NetworkConnection conn) {
            base.OnClientDisconnect(conn);
            ChangeTo(mainMenuPanel);
        }

        public override void OnClientError(NetworkConnection conn, int errorCode) {
            ChangeTo(mainMenuPanel);
            infoPanel.Display("Cient error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()), "Close", null);
        }
    }
}
