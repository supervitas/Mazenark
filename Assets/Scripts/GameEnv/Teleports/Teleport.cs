﻿using UnityEngine;
using UnityEngine.Networking;

namespace GameEnv.Teleports {
    public class Teleport : NetworkBehaviour {
        private Teleport _teleportTo;

        public void SetTeleportTo(Teleport teleport) {
            _teleportTo = teleport;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (!isServer) return;
            
            if (other.CompareTag("Player")) {
                TeleportPlayer(other.gameObject);
            }
        }                

        private void TeleportPlayer(GameObject player) {
            var pos = _teleportTo.transform.position;
            pos.x += 2f;
            pos.z += 2f;
            player.transform.position = pos;
        }
    }
}