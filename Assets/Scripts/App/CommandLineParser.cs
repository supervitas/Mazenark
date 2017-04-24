using System;
using Lobby;
using UnityEngine;

namespace App {
    public class CommandLineParser: MonoBehaviour {

        bool _client = false;

        string _serverAddress = "127.0.0.1";
        int _serverPort = 25000;

        string[] args;

        void Start() {
            args = Environment.GetCommandLineArgs();
            var isServer = GetArg("-server");
            if (isServer == null) return;

            var port = GetArg("-port");

            if (port == null) {
                Debug.LogError("No Port Provided");
                Application.Quit();
            }

            _serverPort = Convert.ToInt32(port);
            FindObjectOfType<LobbyMainMenu>().StartDedicatedServerInstance(_serverPort);
        }

        private string GetArg(string argName) {
            for (var i = 0; i < args.Length; i++) {
                if (args[i] == argName && args.Length > i + 1) {
                    return args[i + 1];
                }
            }
            return null;
        }
    }
}