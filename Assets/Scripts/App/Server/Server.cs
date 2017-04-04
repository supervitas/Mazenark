using UnityEngine;
using UnityEngine.Networking;

namespace App.Server {
    public class Server : NetworkBehaviour {
        public override void OnStartServer() {
            AppManager.Instance.MazeSize.GenerateRndSize();
            AppManager.Instance.MazeInstance = new MazeBuilder.MazeBuilder(AppManager.Instance.MazeSize.X, AppManager.Instance.MazeSize.Y);
            AppManager.Instance.EventHub.CreateEvent("MazeCreated", null);
        }

    }
}