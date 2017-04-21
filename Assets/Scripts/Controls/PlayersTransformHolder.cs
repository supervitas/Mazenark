using System.Collections.Generic;
using UnityEngine;

namespace Controls {
    public class PlayersTransformHolder : MonoBehaviour {
        public readonly List<Transform> PlayersTransform = new List<Transform>();

        public void Start() {
            Invoke("FillPlayers", 4);
        }

        private void FillPlayers() {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
                PlayersTransform.Add(player.transform);
            }
        }

    }
}