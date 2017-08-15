using System.Collections.Generic;
using GameEnv.GameEffects;
using Ui;
using UnityEngine;
using UnityEngine.Networking;

namespace Loot {
    public class Chest : NetworkBehaviour {   
   
        private readonly Dictionary<string, int> _serverChestItems = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _clientChestItems = new Dictionary<string, int>();
  

        public void SetChestItems(string itemName, int count) {            
            _serverChestItems.Add(itemName, count);
        }

        [Command]
        private void CmdSendChestItems() {
            foreach (var item in _serverChestItems) {
                RpcSetPlayerItems(item.Key, item.Value);
            }
        }
        
        [Client]
        private void Start() {
            CmdSendChestItems();
        }


        [Client]
        private void OnTriggerEnter(Collider other) {                       
            if (_clientChestItems.Count == 0) return;
            
            if (other.CompareTag("Player")) {
                FindObjectOfType<PickupItemsGui>().TurnOn(_clientChestItems);
                
//                Destroy(gameObject, 3f);
            }
            
        }
        
        [Client]
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                FindObjectOfType<PickupItemsGui>().TurnOff();
            }
        }

        [ClientRpc]
        private void RpcSetPlayerItems(string item, int count) {            
            _clientChestItems.Add(item, count);
        }
           

        private void BeginDisolve() {
            GetComponent<Disolve>().BeginDisolve(1.0f);
        }
        
    }
}