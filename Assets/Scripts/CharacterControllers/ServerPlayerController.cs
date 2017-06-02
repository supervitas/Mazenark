using Controls;
using Loot;
using UnityEngine;

namespace CharacterControllers {
    public class ServerPlayerController : ServerCharacterController {
        
        private PlayerControl _characterControl;
        
        private void Start() {
            IsNpc = false;            
            InvokeRepeating("PlayerUpdate", 0, 0.5f);
            
            _characterControl = GetComponent<PlayerControl>();
            SetPlayerItems("Fireball", 5);
            SetPlayerItems("Tornado", 3);
        }

        private void SetPlayerItems(string itemName, int itemCount) {
            _characterControl.ServerSetItems(itemName, itemCount);
            _characterControl.TargetSetPlayerItems(connectionToClient, itemName, itemCount);
        }
        
        private void OnDestroy() {            
            CancelInvoke("PlayerUpdate");            
        }     
        
        private void OnTriggerEnter(Collider other) { // take loot
            if (!isServer) return;            
            var go = other.gameObject;
            if (!go.CompareTag("Pickable")) return;
            var lootName = go.GetComponent<LootData>().lootName;
            SetPlayerItems(lootName, 1);          
            Destroy(go);
        }
        
        private void PlayerUpdate() {
            if (transform.position.y < -2.5) {
                TakeDamage(100);
            }
        }
    }
}