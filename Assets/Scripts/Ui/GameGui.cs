using System.Collections.Generic;
using System.Linq;
using App.Eventhub;
using Constants;
using Loot;
using Ui.Drag;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class GameGui: MonoBehaviour {
        [SerializeField] private Image FirstItemImage;
        [SerializeField] private Text FirstItemCount;        
        [SerializeField] private Image SecondItemImage;
        [SerializeField] private Text SecondItemCount;
        [SerializeField] private Image ThirdItemImage;
        [SerializeField] private Text ThirdItemCount;

        private readonly List<UiItem> _uiItemsList = new List<UiItem>();        

        private Image _activeItem;
        private PickupItemsGui _pickupItemsGui;

        private DragAndDropCell.DropDescriptor _previusDropDescriptor;        

        private void Awake() {
            _uiItemsList.Add(new UiItem {itemImage = FirstItemImage, itemCountText = FirstItemCount, itemNumber = 1});
            _uiItemsList.Add(new UiItem {itemImage = SecondItemImage, itemCountText = SecondItemCount, itemNumber = 2});
            _uiItemsList.Add(new UiItem {itemImage = ThirdItemImage, itemCountText = ThirdItemCount, itemNumber = 3});
            
            _pickupItemsGui = FindObjectOfType<PickupItemsGui>();            
        }
        
        
        public void AddItem(string itemName, string count) {
            var item = GetEmptyUiItem(itemName);
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
            placer.itemImage.enabled = true;            
            placer.itemImage.sprite = ItemsCollection.Instance.GetIconByName(itemName);
            placer.itemCountText.text = count;                 
            placer.itemCountText.enabled = true;            
        }
                

        private UiItem GetEmptyUiItem(string itemName) {
//            foreach (var item in _uiItemsList) {
//                if (item.itemName == itemName) {
//                    return item;
//                }
//            }
            return _uiItemsList.FirstOrDefault(item => !item.itemCountText.enabled);
        }

        private UiItem GetItem(string itemName) {
            return (from uiItem in _uiItemsList where uiItem.itemName == itemName select uiItem).FirstOrDefault();
        }
        
        private UiItem GetItem(int number) {
            return (from uiItem in _uiItemsList where uiItem.itemNumber == number select uiItem).FirstOrDefault();
        }
        
        private UiItem GetItem(Image image) {
//            foreach (var item in _uiItemsList) {
//                if (item.itemImage != null) {
//                    if (item.itemImage == image) {
//                        return item;
//                    }
//                }
//                if (item.itemImage == null) {
//                    Debug.Log("NULL");
//                    return item;
//                }
//            }
//            return null;
            return _uiItemsList.FirstOrDefault(uiItem => uiItem.itemImage == image);
        }

        public void OnSheetChange(DragAndDropCell.DropDescriptor desc) {
            if (desc.destinationCell == _previusDropDescriptor.sourceCell) return;
            _previusDropDescriptor = desc;
            
            var image = desc.destinationCell.GetItem().GetComponent<Image>();
            image.enabled = false;
            var pickedItem = _pickupItemsGui.GetRelatedUiItem(image);

            var parent = desc.destinationCell.GetItem().transform.parent.gameObject;
            switch (parent.name) {
                case "FirstItemHolder": {
                    var item = GetItem(1);
                    item.itemImage = image;
                    break;
                }
                case "SecondItemHolder": {
                    var item = GetItem(2);
                    item.itemImage = image;
                    break;
                }
                case "ThirdItemHolder": {
                    var item = GetItem(2);
                    item.itemImage = image;
                    break;
                }
            }            
            _pickupItemsGui.UiItemWasDragedToPlayer(pickedItem);
        }

        private void DragFromChest(DragAndDropCell.DropDescriptor desc) {
            
            var image = desc.destinationCell.GetItem().GetComponent<Image>();            
//            image.enabled = false;
            
            var pickedItem = _pickupItemsGui.GetRelatedUiItem(image);            
//            var whereItDroped = GetItem(image);
            
            _pickupItemsGui.UiItemWasDragedToPlayer(pickedItem);
        }

        public void OnItemPlace(DragAndDropCell.DropDescriptor desc) {
            if (desc.destinationCell == _previusDropDescriptor.sourceCell) return;

            _previusDropDescriptor = desc;
                           

            if (desc.sourceCell.GetComponentInParent<DummyControlUnit>() !=
                desc.destinationCell.GetComponentInParent<DummyControlUnit>()) { // from chest                
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