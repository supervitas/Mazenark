using System;
using System.Collections.Generic;
using System.Linq;
using App;
using Constants;
using MazeBuilder;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Biome = MazeBuilder.Biome;
using Maze = MazeBuilder.Maze;

namespace Lobby {
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
    public class LobbyPlayer : NetworkLobbyPlayer {
        static Color[] Colors = { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
        //used on server to avoid assigning the same color to two player
        static List<int> _colorInUse = new List<int>();

        public Button colorButton;
        public InputField nameInput;
        public Button readyButton;
        public Button waitingPlayerButton;
        public Button removePlayerButton;

        public GameObject localIcone;
        public GameObject remoteIcone;

        //OnMyName function will be invoked on clients when server change the value of playerName
        [SyncVar(hook = "OnMyName")]
        public string playerName = "";
        [SyncVar(hook = "OnMyColor")]
        public Color playerColor = Color.white;

        public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

        static Color JoinColor = new Color(255.0f/255.0f, 0.0f, 101.0f/255.0f,1.0f);
        static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
        static Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
        static Color TransparentColor = new Color(0, 0, 0, 0);

        private Maze _fetchedMaze;

        public struct MazeStruct {
            public int X;
            public int Y;
            public string BiomeName;
            public int TileType;
            public int BiomeInstanceId;

            public MazeStruct(int x, int y, string biomeName, int tileType, int biomeInstanceId) {
                X = x;
                Y = y;
                BiomeName = biomeName;
                TileType = tileType;
                BiomeInstanceId = biomeInstanceId;
            }
        }



        public override void OnClientEnterLobby() {
            base.OnClientEnterLobby();

            if (LobbyManager.SSingleton != null) LobbyManager.SSingleton.OnPlayersNumberModified(1);

            LobbyPlayerList._instance.AddPlayer(this);
            LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.SSingleton.matchMaker == null);

            if (isLocalPlayer) {
                SetupLocalPlayer();
            } else {
                SetupOtherPlayer();
            }

            //setup the player data on UI. The value are SyncVar so the player
            //will be created with the right value currently on server
            OnMyName(playerName);
            OnMyColor(playerColor);
        }

        public override void OnStartAuthority() {
            base.OnStartAuthority();

            //if we return from a game, color of text can still be the one for "Ready"
            readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

           SetupLocalPlayer();
        }

        void ChangeReadyButtonColor(Color c)
        {
            ColorBlock b = readyButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
            b.disabledColor = c;
            readyButton.colors = b;
        }

        void SetupOtherPlayer() {
            nameInput.interactable = false;
            removePlayerButton.interactable = NetworkServer.active;

            ChangeReadyButtonColor(NotReadyColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            readyButton.interactable = false;

            OnClientReady(false);
        }

        void SetupLocalPlayer() {
            remoteIcone.gameObject.SetActive(false);
            localIcone.gameObject.SetActive(true);

            var user = AppLocalStorage.Instance.user;
            var playerNick = user.username;
            if (playerNick.Length > 12) {
                playerNick = name.Substring(0, 12);
                playerNick += "...";
            }

            playerName = playerNick;

            if (AppManager.Instance.IsSinglePlayer) {
                OnReadyClicked();
                return;
            }

//            CmdCheckToken(user.token);

            CmdNameChanged(playerNick);

            CmdGetMaze();

            CheckRemoveButton();

            if (playerColor == Color.white)
                CmdColorChange();

            ChangeReadyButtonColor(JoinColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
            readyButton.interactable = true;

            //have to use child count of player prefab already setup as "this.slot" is not set yet
            if (playerName == "")
                CmdNameChanged("Player" + (LobbyPlayerList._instance.playerListContentTransform.childCount-1));


            //we switch from simple name display to name input
            colorButton.interactable = true;
//            nameInput.interactable = true;

            nameInput.onEndEdit.RemoveAllListeners();
            nameInput.onEndEdit.AddListener(OnNameChanged);

            colorButton.onClick.RemoveAllListeners();
            colorButton.onClick.AddListener(OnColorClicked);

            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(OnReadyClicked);

            //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
            //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            if (LobbyManager.SSingleton != null) LobbyManager.SSingleton.OnPlayersNumberModified(0);
        }

        //This enable/disable the remove button depending on if that is the only local player or not
        public void CheckRemoveButton() {
            if (!isLocalPlayer)
                return;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            removePlayerButton.interactable = localPlayerCount > 1;

        }

        public override void OnClientReady(bool readyState) {
            if (readyState) {
                ChangeReadyButtonColor(TransparentColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = "READY";
                textComponent.color = ReadyColor;
                readyButton.interactable = false;
                colorButton.interactable = false;
                nameInput.interactable = false;
            } else {
                ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = isLocalPlayer ? "JOIN" : "...";
                textComponent.color = Color.white;
                readyButton.interactable = isLocalPlayer;
                colorButton.interactable = isLocalPlayer;
                nameInput.interactable = isLocalPlayer;
            }
        }

        public void OnPlayerListChanged(int idx) {
            GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
        }

        ///===== callback from sync var

        public void OnMyName(string newName) {
            playerName = newName;
            nameInput.text = playerName;
        }

        public void OnMyColor(Color newColor) {
            playerColor = newColor;
            colorButton.GetComponent<Image>().color = newColor;
        }

        //===== UI Handler

        //Note that those handler use Command function, as we need to change the value on the server not locally
        //so that all client get the new value throught syncvar
        public void OnColorClicked()
        {
            CmdColorChange();
        }

        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        public void OnNameChanged(string str) {
            CmdNameChanged(str);
        }

        public void OnRemovePlayerClick() {
            if (isLocalPlayer) {
                RemovePlayer();
            } else if (isServer)
                LobbyManager.SSingleton.KickPlayer(connectionToClient);
                
        }

        public void ToggleJoinButton(bool enabled) {
            readyButton.gameObject.SetActive(enabled);
            waitingPlayerButton.gameObject.SetActive(!enabled);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown) {
            LobbyManager.SSingleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.SSingleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        [ClientRpc]
        public void RpcUpdateRemoveButton() {
            CheckRemoveButton();
        }

        private Biome GetBiomeByName(string biomeName) {
            return Biome.AllBiomesList.First(bm => bm.Name == biomeName);
        }

        [TargetRpc]
        public void TargetCreateMaze(NetworkConnection target, int width, int height) {
            _fetchedMaze = new Maze(width, height, true);
        }

        [TargetRpc]
        private void TargetFillMaze(NetworkConnection target, MazeStruct[] mazeArr) {

            foreach (var tile in mazeArr) {
                _fetchedMaze[tile.X, tile.Y].Biome = GetBiomeByName(tile.BiomeName);
                _fetchedMaze[tile.X, tile.Y].Type = (Tile.Variant) tile.TileType;
                _fetchedMaze[tile.X, tile.Y].BiomeID = tile.BiomeInstanceId;
            }
        }

        [TargetRpc]
        public void TargetMazeLoadingFinished(NetworkConnection target, int width, int hight, int maxBiomeID) {
            AppManager.Instance.MazeInstance = new MazeBuilder.MazeBuilder(width, hight, _fetchedMaze);
            AppManager.Instance.MazeInstance.Maze.GenerateBiomesList();
        }

        //====== Server Command

        [Command]
        private void CmdGetMaze() {
            if (!isServer)
                return;

            var messageBatchSize = 10; // how much rows will be send in one message;
            var biomeList = new List<MazeStruct>();

            var mazeInstance = AppManager.Instance.MazeInstance;
            var maze = mazeInstance.Maze;

            TargetCreateMaze(connectionToClient, maze.Width, maze.Height); // create maze

            var counter = 0;
            for (var x = 0; x < mazeInstance.Height; x++) {
                for (var y = 0; y < mazeInstance.Width; y++) {
                    biomeList.Add(new MazeStruct(x, y, maze[x, y].Biome.Name, (int) maze[x, y].Type, maze[x, y].BiomeID)); // fill maze
                }
                counter++;
                if (counter >= messageBatchSize) {
                    TargetFillMaze(connectionToClient, biomeList.ToArray());
                    counter = 0;
                    biomeList.Clear();
                }
            }
            TargetFillMaze(connectionToClient, biomeList.ToArray()); // send final chunk of data

            TargetMazeLoadingFinished(connectionToClient, maze.Width, maze.Height, maze.MaxBiomeID);
        }

        [TargetRpc]
        public void TargetDropAuth(NetworkConnection target) {
            AppLocalStorage.Instance.ResetAuth();
        }

        [Command]
        private void CmdCheckToken(string token) {
            if (!isServer)
                return;

            Action<string> errorCb = error => {
                TargetDropAuth(connectionToClient);
                LobbyManager.SSingleton.KickPlayer(connectionToClient);

            };
            
            Action<string> resultCb = data => {
                Debug.Log(data);                

            };

            NetworkHttpManager.Instance.GetUserData(NetworkConstants.UserByToken, new Token {token = token}, resultCb, errorCb);
        }

        [Command]
        public void CmdColorChange() {
            int idx = Array.IndexOf(Colors, playerColor);

            int inUseIdx = _colorInUse.IndexOf(idx);

            if (idx < 0) idx = 0;

            idx = (idx + 1) % Colors.Length;

            bool alreadyInUse = false;

            do {
                alreadyInUse = false;
                foreach (var t in _colorInUse) {
                    if (t == idx)
                    {//that color is already in use
                        alreadyInUse = true;
                        idx = (idx + 1) % Colors.Length;
                    }
                }
            }
            while (alreadyInUse);

            if (inUseIdx >= 0)
            {//if we already add an entry in the colorTabs, we change it
                _colorInUse[inUseIdx] = idx;
            }
            else {//else we add it
                _colorInUse.Add(idx);
            }

            playerColor = Colors[idx];
        }

        [Command]
        public void CmdNameChanged(string name) {
            playerName = name;
        }

        //Cleanup thing when get destroy (which happen when client kick or disconnect)
        public void OnDestroy() {
            if (LobbyPlayerList._instance) {
                LobbyPlayerList._instance.RemovePlayer(this);
                if (LobbyManager.SSingleton != null) LobbyManager.SSingleton.OnPlayersNumberModified(-1);

                int idx = Array.IndexOf(Colors, playerColor);

                if (idx < 0)
                    return;

                for (int i = 0; i < _colorInUse.Count; ++i) {
                    if (_colorInUse[i] == idx) { //that color is already in use
                        _colorInUse.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
