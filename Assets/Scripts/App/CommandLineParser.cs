using System;
using Lobby;
using UnityEngine;

namespace App {
    public class CommandLineParser: MonoBehaviour {
        private string[] _args;

        private void Start() {
            _args = Environment.GetCommandLineArgs();
            var isServer = GetArg("-server");
            if (isServer == null) return;

            var port = GetArg("-port");
            var instanceId = GetArg("-instanceid");

            if (port == null || instanceId == null) {
                Debug.LogError("No port or instanceid provided");
                Application.Quit();
            }

            FindObjectOfType<LobbyManager>().StartDedicatedServerInstance(Convert.ToInt32(port), Convert.ToInt32(instanceId));
        }

        private string GetArg(string argName) {
            for (var i = 0; i < _args.Length; i++) {
                if (_args[i] == argName && _args.Length > i + 1) {
                    return _args[i + 1];
                }
            }
            return null;
        }
    }
}