using System.Collections.Generic;
using System.Linq;
using App.Eventhub;
using Loot;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class PickupItemsGui: MonoBehaviour {
        
        [SerializeField] private Image FirstItemImage;
        [SerializeField] private Text FirstItemCount;
        [SerializeField] private Image SecondItemImage;
        [SerializeField] private Text SecondItemCount;
        [SerializeField] private Image ThirdItemImage;
        [SerializeField] private Text ThirdItemCount;

        private readonly List<UiItem> _uiItemsList = new List<UiItem>();

        private Image _activeItem;

        internal class UiItem {
            public Image itemImage;
            public Text itemCountText;
            public string itemName;
            public int itemNumber;
        }

        private void Start() {
            _uiItemsList.Add(new UiItem {itemImage = FirstItemImage, itemCountText = FirstItemCount, itemNumber = 1});
            _uiItemsList.Add(new UiItem {itemImage = SecondItemImage, itemCountText = SecondItemCount, itemNumber = 2});
            _uiItemsList.Add(new UiItem {itemImage = ThirdItemImage, itemCountText = ThirdItemCount, itemNumber = 3});                                  
        }
        
        public void AddItem(string itemName, string count) {
            var item = GetEmptyUiItem();
            item.itemName = itemName;
            CreateItem(itemName, count,item);
        }

        public void SetActiveItem(int itemNumber) {            
            var uiItem = GetItem(itemNumber);            
            if (uiItem.itemName == null) return;
            if (_activeItem != null) {
                _activeItem.color = new Color32(197, 184, 204, 81);
            }
            _activeItem = uiItem.itemImage.transform.GetComponentInParent<Image>();
            _activeItem.color = Color.yellow;
            App.AppManager.Instance.EventHub.CreateEvent("ItemChanged", new EventArguments(uiItem.itemName));
        }

        public void ModifyItemCount(string itemName, string count) {
            var uiItem = GetItem(itemName);       
            uiItem.itemCountText.text = count;
        }
        
        public void DisableItem(string itemName) {
            var uiItem = GetItem(itemName);
            uiItem.itemImage.enabled = false;
            uiItem.itemCountText.enabled = false;
            _activeItem.color = new Color32(197, 184, 204, 81);
            _activeItem = null;
            uiItem.itemName = null;
        }

        private void CreateItem(string itemName, string count, UiItem placer) {            
            placer.itemImage.sprite = ItemsCollection.Instance.GetIconByName(itemName);
            placer.itemCountText.text = count;
            placer.itemImage.enabled = true;
            placer.itemCountText.enabled = true;
        }

        private UiItem GetEmptyUiItem() {
            foreach (var item in _uiItemsList) {
                if (item.itemImage.enabled) continue;                
                return item;
            }
            return new UiItem();
        }

        private UiItem GetItem(string itemName) {
            return (from uiItem in _uiItemsList where uiItem.itemName == itemName select uiItem).FirstOrDefault();
        }
        private UiItem GetItem(int number) {
            return (from uiItem in _uiItemsList where uiItem.itemNumber == number select uiItem).FirstOrDefault();
        }

    }
}