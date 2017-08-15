using System.Collections.Generic;
using GameEnv.GameEffects;
using Ui;
using UnityEngine;
using UnityEngine.Networking;

namespace Loot {
    public class Chest : NetworkBehaviour {   
   
        private readonly Dictionary<string, int> _serverChestItems = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _clientChestItems = new Dictionary<string, int>();
        

        private static PickupItemsGui _pickupItemsGui;

        public void SetChestItems(string itemName, int count) {            
            _serverChestItems.Add(itemName, count);
        }            
        
        [Command]
        private void CmdSendChestItems() {
            foreach (var item in _serverChestItems) {
                RpcSetChestItems(item.Key, item.Value);
            }
        }
        
        [Command]
        private void CmdItemPicked(string itemName) {
            _serverChestItems.Remove(itemName);
            RpcItemPicked(itemName);
            if (_serverChestItems.Count == 0) {
                RpcDestruct(2f);
                Destroy(gameObject, 3f);
            }
        }

        
        [Client]
        public void ItemPicked(string itemName) {
            CmdItemPicked(itemName);
        }   
        
        [Client]
        private void Start() {
            CmdSendChestItems();
            _pickupItemsGui = FindObjectOfType<PickupItemsGui>();
        }

        [Client]
        private void OnTriggerEnter(Collider other) {                       
            if (_clientChestItems.Count == 0) return;
            
            if (other.CompareTag("Player")) {
                _pickupItemsGui.TurnOn(_clientChestItems, this);                
            }            
        }
        
        [Client]
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                _pickupItemsGui.TurnOff();
            }
        }

        [ClientRpc]
        private void RpcDestruct(float timeOfDestruct) {
            gameObject.GetComponent<Collider>().enabled = false;
            Invoke(nameof(BeginDisolve), timeOfDestruct / 2);
        }

        [ClientRpc]
        private void RpcSetChestItems(string item, int count) {
            _clientChestItems.Add(item, count);
        }

        [ClientRpc]
        private void RpcItemPicked(string itemName) {
            _clientChestItems.Remove(itemName);
            if (_clientChestItems.Count == 0) {
                _pickupItemsGui.TurnOff();
            }
        }
           

        private void BeginDisolve() {
            GetComponent<Disolve>().BeginDisolve(1.5f);
        }
    }
}