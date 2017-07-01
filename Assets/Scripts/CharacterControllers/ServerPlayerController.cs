using App;
using App.Eventhub;
using Controls;
using GameSystems;
using Lobby;
using Loot;
using UnityEngine;

namespace CharacterControllers {
    public class ServerPlayerController : ServerCharacterController {
        
        private PlayerControl _characterControl;
        
        private void Start() {
            if (!isServer) return;
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
        
        public override void TakeDamage(int amount, float timeOfDeath = 2f) {            
            if (!isServer) return;
            CurrentHealth -= amount;
            if (CurrentHealth > 0) return;
            CurrentHealth = 0;
            
            FindObjectOfType<GameManager>().PlayerDied(gameObject);
            NetworkEventHub.Instance.RpcPublishEvent("PlayerDied", gameObject.name);                        
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