using System.Collections.Generic;
using System.Linq;
using App.Eventhub;
using Loot;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class PickupItemsGui: MonoBehaviour {
        [SerializeField] private Canvas CanvasObject;
        
        [SerializeField] private Image FirstItemImage;
        [SerializeField] private Text FirstItemCount;        
        [SerializeField] private Image SecondItemImage;
        [SerializeField] private Text SecondItemCount;
        [SerializeField] private Image ThirdItemImage;
        [SerializeField] private Text ThirdItemCount;

        private readonly List<UiItem> _uiItemsList = new List<UiItem>();

        private Image _activeItem;

        private DragAndDropCell.DropDescriptor _previusDropDescriptor;
        
        internal class UiItem {
            public Image itemImage;
            public Text itemCountText;
            public string itemName;            
        }
        
        private void Awake() {
            _uiItemsList.Add(new UiItem {itemImage = FirstItemImage, itemCountText = FirstItemCount});
            _uiItemsList.Add(new UiItem {itemImage = SecondItemImage, itemCountText = SecondItemCount});
            _uiItemsList.Add(new UiItem {itemImage = ThirdItemImage, itemCountText = ThirdItemCount});            
        }

        public void TurnOn(Dictionary<string, int> items) {
            foreach (var uiItem in _uiItemsList) {
                uiItem.itemImage.enabled = false;
                uiItem.itemCountText.enabled = false;                               
            }          
            
            foreach (var item in items) {
                AddItem(item.Key, item.Value.ToString());
            }
            
            CanvasObject.enabled = true;            
        }

        public void TurnOff() {
            CanvasObject.enabled = false;
        }       
        
        public void AddItem(string itemName, string count) {
            var item = GetEmptyUiItem();
            item.itemName = itemName;
            CreateItem(itemName, count,item);
        }

        private void CreateItem(string itemName, string count, UiItem placer) {            
            placer.itemImage.sprite = ItemsCollection.Instance.GetIconByName(itemName);
            placer.itemCountText.text = count;
            placer.itemImage.enabled = true;
            placer.itemCountText.enabled = true;
        }

        private UiItem GetEmptyUiItem() {
            return _uiItemsList.FirstOrDefault(item => !item.itemImage.enabled);
        }
       
    }
}