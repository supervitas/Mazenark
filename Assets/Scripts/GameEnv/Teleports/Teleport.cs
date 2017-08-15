using CharacterControllers;
using UnityEngine;
using UnityEngine.Networking;

namespace GameEnv.Teleports {
    public class Teleport : NetworkBehaviour {
        private Teleport _teleportTo;
        public GameObject TeleportedPlayer;

        public void SetTeleportTo(Teleport teleport) {
            _teleportTo = teleport;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (!isServer) return;
                         
            if (other.CompareTag("Player") && other.gameObject != TeleportedPlayer) {
                _teleportTo.TeleportedPlayer = other.gameObject;
                TeleportPlayer(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (!isServer) return;
            
            if (other.CompareTag("Player") && other.gameObject == TeleportedPlayer) {
                TeleportedPlayer = null;
            }
        }

        private void TeleportPlayer(GameObject player) {           
            if (!isServer) return;
            
            var pos = _teleportTo.transform.position;            
            player.transform.position = pos;
        }
    }
}