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
        
        [SyncVar(hook = "OnSetName")] private string _playerName;
        
        private PlayerControl _characterControl;
        private readonly Dictionary<string, int> _playerItems = new Dictionary<string, int>();        
        private GameObject _activeItem;
                
        private void Start() {
            if (!isServer) return;
            
            IsNpc = false;            
            InvokeRepeating("PlayerUpdate", 0, 0.5f);
            
            _characterControl = GetComponent<PlayerControl>();
            
            SetPlayerItems("Fireball", 5);
            SetPlayerItems("Tornado", 3);
        }
                
        private void OnSetName(string playerName) {            
            if (isLocalPlayer) return;
            var textMesh = GetComponentInChildren<TextMesh>();
            textMesh.text = playerName;
        }

        private void SetPlayerItems(string itemName, int itemCount) {
            if (!_playerItems.ContainsKey(itemName)) {               
                _playerItems.Add(itemName, 0);
            }
            _playerItems[itemName] += itemCount;
            
            _characterControl.TargetSetPlayerItems(connectionToClient, itemName, itemCount);
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
            if (!isServer) return;
            
            if (transform.position.y < -2.5) {
                TakeDamage(100);
            }
        }
        
        
        [Command]
        public void CmdFire(Vector3 direction) {
            if (_playerItems[_activeItem.name] <= 0) return;
            
            _playerItems[_activeItem.name]--;
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
                        
        [Command]
        public void CmdNameChanged(string name) {
            _playerName = name;
        }
    }
    
}