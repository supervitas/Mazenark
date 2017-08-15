using System.Collections.Generic;
using System.Linq;
using App.Eventhub;
using Constants;
using Loot;
using Ui.Drag;
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
        private Chest _activeChest;
           
        
        private void Awake() {
            _uiItemsList.Add(new UiItem {itemImage = FirstItemImage, itemCountText = FirstItemCount, itemNumber = 1});
            _uiItemsList.Add(new UiItem {itemImage = SecondItemImage, itemCountText = SecondItemCount, itemNumber = 2});
            _uiItemsList.Add(new UiItem {itemImage = ThirdItemImage, itemCountText = ThirdItemCount, itemNumber = 3});            
        }

        public void TurnOn(Dictionary<string, int> items, Chest chest) {
            _activeChest = chest;
            foreach (var item in items) {
                AddItem(item.Key, item.Value.ToString());
            }
            
            CanvasObject.enabled = true;            
        }

        public void TurnOff() {
            CanvasObject.enabled = false;
            foreach (var uiItem in _uiItemsList) {
                uiItem.itemImage.enabled = false;
                uiItem.itemCountText.enabled = false;
            }
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
        
        private UiItem GetItem(Image image) {
            return _uiItemsList.FirstOrDefault(uiItem => uiItem.itemImage == image);
        }
        
        private UiItem GetItem(int number) {
            return (from uiItem in _uiItemsList where uiItem.itemNumber == number select uiItem).FirstOrDefault();
        }
        
        public void UiItemWasDragedToPlayer(UiItem item) {
            
//            item.itemCountText.enabled = false;
//            item.itemImage.enabled = false;            
            _activeChest.ItemPicked(item.itemName);
        }

        public UiItem GetRelatedUiItem(Image image) {
            return GetItem(image);
        }          

        public void OnSheetChange(DragAndDropCell.DropDescriptor desc) {
            if (desc.destinationCell == _previusDropDescriptor.sourceCell) return;
            _previusDropDescriptor = desc;
            
            var image = desc.sourceCell.GetItem().GetComponent<Image>();
            image.enabled = false;            

            var parent = desc.sourceCell.GetItem().transform.parent.gameObject;
            switch (parent.name) {
                case "FirstItem": {
                    var item = GetItem(1);
                    item.itemImage = image;
                    break;
                }
                case "SecondItem": {
                    var item = GetItem(2);
                    item.itemImage = image;
                    break;
                }
                case "ThirdItem": {
                    var item = GetItem(2);
                    item.itemImage = image;
                    break;
                }
            }
        }
        
        public void OnItemPlace(DragAndDropCell.DropDescriptor desc) {
            if (desc.destinationCell == _previusDropDescriptor.sourceCell) return;

            _previusDropDescriptor = desc;
                           

            if (desc.sourceCell.GetComponentInParent<DummyControlUnit>() !=
                desc.destinationCell.GetComponentInParent<DummyControlUnit>()) {       
                return;
            }
            
            var sourceSprite = desc.sourceCell.GetItem().GetComponent<Image>();
            var destinationSprite = desc.item.GetComponent<Image>();
            
            var sourceUiItem = GetItem(sourceSprite);
            var destinationUiItem = GetItem(destinationSprite);

            var sourceItemCount = sourceUiItem.itemCountText.text;
            var destinationItemCount = destinationUiItem.itemCountText.text;

            var sourceItemName = sourceUiItem.itemName;
            var destinationItemName = destinationUiItem.itemName;
                 
            sourceUiItem.itemName = destinationItemName;            
            destinationUiItem.itemName = sourceItemName;                                                      
            
            sourceUiItem.itemCountText.text = destinationItemCount;
            destinationUiItem.itemCountText.text = sourceItemCount;

            sourceUiItem.itemImage = destinationSprite;
            destinationUiItem.itemImage = sourceSprite;


            if (sourceUiItem.itemCountText.enabled == false) {
                destinationUiItem.itemCountText.enabled = false;
                sourceUiItem.itemCountText.enabled = true;
            } else {
                if (destinationUiItem.itemCountText.enabled == false) {
                    sourceUiItem.itemCountText.enabled = false;
                    destinationUiItem.itemCountText.enabled = true;
                }
            }
            
        }

    }
}