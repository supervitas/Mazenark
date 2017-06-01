using System.Linq;
using UnityEngine;

namespace Loot {
    public class ItemsCollection : MonoBehaviour {
        public static ItemsCollection Instance; 
        public GameObject[] items;
        public Texture[] itemIcons;

        private void Start() {
            if (Instance == null) {
                Instance = this;
            }
        }
          
        public GameObject GetItemByName(string itemName) {
            return items.FirstOrDefault(item => item.name == itemName);
        }

        public Texture GetIconByName(string itemName) {
            return itemIcons.FirstOrDefault(item => item.name == itemName);
        }
    }
}