using System.Collections.Generic;
using System.Linq;
using CharacterControllers;
using GameEnv.GameEffects;
using Ui;
using UnityEngine;
using UnityEngine.Networking;

namespace Loot {
    public class Chest : NetworkBehaviour {
        private struct ChestItems {
            public string ItemName;
            public int Count;
        }
        private class SyncListChestItems : SyncListStruct<ChestItems> {}
        private readonly SyncListChestItems _chestItems = new SyncListChestItems();        
      

        private ServerPlayerController _activePlayer;
        
        private static PickupItemsGui _pickupItemsGui;

        public void SetChestItems(string itemName, int count) {         
            _chestItems.Add(new ChestItems {
                Count = count,
                ItemName = itemName
            });            
        }                    
        
        [Command]
        private void CmdItemPlaced(string itemName, int itemCount) {
            _activePlayer.RemovePlayerItem(itemName);
            var item = _chestItems.FirstOrDefault(it => it.ItemName == itemName);
            var index = _chestItems.IndexOf(item);
            var count = item.Count;

            if (item.ItemName == itemName) {
                var newItem = new ChestItems {
                    Count = count + itemCount,
                    ItemName = itemName
                };
                _chestItems.Remove(item);
                _chestItems.Insert(index, newItem);
            } else {
                _chestItems.Add(new ChestItems {
                    Count = itemCount,
                    ItemName = itemName
                }); 
            }        
            
            TargetUpdateGui(_activePlayer.connectionToClient);
        }
        
        [Command]
        private void CmdItemPicked(string itemName) {
            var item = _chestItems.FirstOrDefault(it => it.ItemName == itemName);
            _activePlayer.SetPlayerItems(item.ItemName, item.Count);
            _chestItems.Remove(item);
            
            if (_chestItems.Count == 0) {
                TargetTurnOffGui(_activePlayer.connectionToClient);
                RpcDestruct(2f);
                Destroy(gameObject, 3f);
            }            
        }

        
        public void ItemPlaced(string itemName, int itemCount) {                        
            CmdItemPlaced(itemName, itemCount);
        }
                
        public void ItemPicked(string itemName) {
            CmdItemPicked(itemName);
        }

        private void Start() {
            if (_pickupItemsGui == null) {
                _pickupItemsGui = FindObjectOfType<PickupItemsGui>();
            }            
        }

        private void OnTriggerEnter(Collider other) {            
            if (!isServer || _chestItems.Count == 0) return;            
           
            if (other.CompareTag("Player")) {
                gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(other.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
                
                _activePlayer = other.gameObject.GetComponent<ServerPlayerController>();
                
                TargetTurnOnGui(_activePlayer.connectionToClient);
            }
        }
        
        
        private void OnTriggerExit(Collider other) {
            if (!isServer) return;
            
            if (other.CompareTag("Player")) {
                gameObject.GetComponent<NetworkIdentity>().RemoveClientAuthority(other.gameObject.GetComponent<NetworkIdentity>().connectionToClient);

                TargetTurnOffGui(_activePlayer.connectionToClient);               
            }
        }

        [ClientRpc]
        private void RpcDestruct(float timeOfDestruct) {           
            gameObject.GetComponent<Collider>().enabled = false;
            Invoke(nameof(BeginDisolve), timeOfDestruct / 2);
        }
          

        [TargetRpc]
        private void TargetTurnOnGui(NetworkConnection target) {            
            _pickupItemsGui.TurnOn(_chestItems.ToDictionary(t => t.ItemName, t => t.Count), this);
        }
        
        [TargetRpc]
        private void TargetUpdateGui(NetworkConnection target) {                       
            _pickupItemsGui.UpdateGui(_chestItems.ToDictionary(t => t.ItemName, t => t.Count));
        }
        
        
        [TargetRpc]
        private void TargetTurnOffGui(NetworkConnection target) {            
            _pickupItemsGui.TurnOff();
        }
        
        private void BeginDisolve() {
            GetComponent<Disolve>().BeginDisolve(1.5f);
        }
    }
}