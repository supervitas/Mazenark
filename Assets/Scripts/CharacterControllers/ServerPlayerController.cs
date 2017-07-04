using System.Collections.Generic;
using App;
using Controls;
using GameSystems;
using Loot;
using UnityEngine;
using UnityEngine.Networking;
using Weapons;

namespace CharacterControllers {
    public class ServerPlayerController : ServerCharacterController {
                        
        private PlayerControl _characterControl;
        private readonly Dictionary<string, int> _serverPlayerItems = new Dictionary<string, int>();        
        private GameObject _activeItem;        

        public override void OnStartServer() {            
            IsNpc = false;
            _characterControl = GetComponent<PlayerControl>();
            InvokeRepeating("PlayerUpdate", 0, 0.5f);
        }

        private void OnDestroy() {
            if (!isServer) return;
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
            if (go.CompareTag("Pickable")) {
                var lootName = go.GetComponent<LootData>().lootName;
                SetPlayerItems(lootName, 1);
                Destroy(go);
            }
        }       
        
        private void PlayerUpdate() {                        
            if (transform.position.y < -2.5) {
                TakeDamage(100);
            }
        }
        
        private void SetPlayerItems(string itemName, int itemCount) {
            if (!isServer) return;  
            
            if (!_serverPlayerItems.ContainsKey(itemName)) {               
                _serverPlayerItems.Add(itemName, 0);
            }
            _serverPlayerItems[itemName] += itemCount;
            
            _characterControl.TargetSetPlayerItems(connectionToClient, itemName, itemCount);
        }        

        [Command]
        public void CmdSetPlayerName(string playerName) {
            _characterControl.RpcSetPlayerName(playerName);
        }

        [Command]
        public void CmdPlayerReady() {            
            SetPlayerItems("Fireball", 5);
            SetPlayerItems("Tornado", 3);
            FindObjectOfType<GameManager>().AddPlayerTransform(transform);
        }


        [Command]
        public void CmdFire(Vector3 direction) {
            if (_serverPlayerItems[_activeItem.name] <= 0) return;
            
            _serverPlayerItems[_activeItem.name]--;
            var pos = transform.position;
            pos.y += 2.3f;
            var activeItem = Instantiate(_activeItem, pos, Quaternion.identity);
            var weapon = activeItem.GetComponent<Weapon>();
            Physics.IgnoreCollision(activeItem.GetComponent<Collider>(), GetComponent<Collider>());
            activeItem.transform.LookAt(direction);
            weapon.Fire();
            NetworkServer.Spawn(activeItem);
            Destroy(weapon, 10.0f);
        }       
        
        [Command]
        public void CmdSetActiveItem(string itemName) {
            _activeItem = ItemsCollection.Instance.GetItemByName(itemName);            
        }                                 
        
    }
    
}