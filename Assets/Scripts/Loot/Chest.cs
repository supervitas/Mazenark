using System.Collections.Generic;
using CharacterControllers;
using GameEnv.GameEffects;
using UnityEngine;
using UnityEngine.Networking;

namespace Loot {
    public class Chest : NetworkBehaviour {

        private readonly Dictionary<string, int> _chestItems = new Dictionary<string, int>();
        
        private void OnTriggerEnter(Collider other) {
            if (!isServer || _chestItems.Count == 0) return;
            
            if (other.CompareTag("Player")) {
                var controller = other.GetComponent<ServerPlayerController>();
                
                foreach (var item in _chestItems) {                    
                    controller.SetPlayerItems(item.Key, item.Value);
                }

//                RpcChestOpened();
//                Destroy(gameObject, 3f);
            }
            
        }

        public void SetChestItems(string itemName, int count) {
            _chestItems.Add(itemName, count);
        }

        [ClientRpc]
        private void RpcChestOpened() {
            GetComponent<Collider>().enabled = false;
            Invoke(nameof(BeginDisolve), 1.0f);
        }

        private void BeginDisolve() {
            GetComponent<Disolve>().BeginDisolve(1.0f);
        }
    }
}