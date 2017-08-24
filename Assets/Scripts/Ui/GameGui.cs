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
            _activeItem = uiItem.itemCountText.transform.GetComponentInParent<Image>();            
            _activeItem.color = Color.yellow;
            App.AppManager.Instance.EventHub.CreateEvent("ItemChanged", new EventArguments(uiItem.itemName));
        }

        public void ModifyItemCount(string itemName, string count) {
            var uiItem = GetItem(itemName);
            uiItem.itemImage.enabled = true;
            uiItem.itemCountText.text = count;
        }
        
        public void DisableItem(string itemName) {            
            var uiItem = GetItem(itemName);            
            uiItem.itemImage.enabled = false;
            uiItem.itemCountText.enabled = false;
            if (_activeItem != null) {
                _activeItem.color = new Color32(197, 184, 204, 81);
                _activeItem = null;
            }
            uiItem.itemName = null;
        }

        private void CreateItem(string itemName, string count, UiItem placer) {           
            placer.itemImage.enabled = true;            
            placer.itemImage.sprite = ItemsCollection.Instance.GetIconByName(itemName);
            placer.itemCountText.text = count;                 
            placer.itemCountText.enabled = true;            
        }
                

        private UiItem GetEmptyUiItem() {
            return _uiItemsList.FirstOrDefault(item => !item.itemCountText.enabled);
        }

        private UiItem GetItem(string itemName) {
            return (from uiItem in _uiItemsList where uiItem.itemName == itemName select uiItem).FirstOrDefault();
        }
        
        private UiItem GetItem(int number) {
            return (from uiItem in _uiItemsList where uiItem.itemNumber == number select uiItem).FirstOrDefault();
        }
        
        private UiItem GetItem(Image image) {
            return _uiItemsList.FirstOrDefault(uiItem => uiItem.itemImage == image);
        }
        
        public UiItem GetRelatedUiItem(Image image) {
            return GetItem(image);
        }

        public void OnSheetChange(DragAndDropCell.DropDescriptor desc) { // draged from chest
            var sourceSprite = desc.sourceCell.GetItem().GetComponent<Image>();
            var destinationSprite = desc.item.GetComponent<Image>();
            
            destinationSprite.enabled = false;
            
            var pickedItem = _pickupItemsGui.GetRelatedUiItem(destinationSprite);
            
            var parent = desc.destinationCell.GetItem().transform.parent.gameObject;

            UiItem item = null;
            
            switch (parent.name) {
                case "FirstItemHolder": {
                    item = GetItem(1);
                    break;
                }
                case "SecondItemHolder": {
                    item = GetItem(2);
                    break;
                }
                case "ThirdItemHolder": {
                    item = GetItem(3);
                    break;
                }
            }     
            
            if (item.itemName != null && pickedItem.itemName != item.itemName) { // swap items;                
                item.itemImage = destinationSprite;
                item.itemImage.transform.rotation = Quaternion.Euler(0, 0, 0);
                
                pickedItem.itemImage = sourceSprite;
                pickedItem.itemImage.transform.rotation = Quaternion.Euler(0, 0, 0);     

                _pickupItemsGui.UiItemWasDragedToChest(item, destinationSprite);
                _pickupItemsGui.UiItemWasDragedToPlayer(pickedItem, sourceSprite);
                return;
            }

            if (item != null) {                
                item.itemImage = destinationSprite;
                item.itemImage.transform.rotation = Quaternion.Euler(0, 0, 0);
            }            

            _pickupItemsGui.UiItemWasDragedToPlayer(pickedItem, sourceSprite);
        }

        public void OnItemPlace(DragAndDropCell.DropDescriptor desc) {
            if (desc.destinationCell == _previusDropDescriptor.sourceCell ||               
                desc.sourceCell.GetComponentInParent<DummyControlUnit>() !=
                desc.destinationCell.GetComponentInParent<DummyControlUnit>()) return;

            _previusDropDescriptor = desc;                                          
            
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