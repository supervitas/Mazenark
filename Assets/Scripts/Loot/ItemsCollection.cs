using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Loot {
    public class ItemsCollection : MonoBehaviour {
        public static ItemsCollection Instance; 
        public GameObject[] items;
        public Sprite[] itemIcons;

        private void Start() {
            if (Instance == null) {
                Instance = this;
            }
        }
          
        public GameObject GetItemByName(string itemName) {
            return items.FirstOrDefault(item => item.name == itemName);
        }

        public Sprite GetIconByName(string itemName) {
            return itemIcons.FirstOrDefault(item => item.name == itemName);
        }

        public GameObject GetRandomLoot() {
            var random = Random.Range(0, items.Length);            
            return items[random];
        }
    }
}