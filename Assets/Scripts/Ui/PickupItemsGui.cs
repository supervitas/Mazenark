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
        
        private GameGui _gameGui;

        private void Awake() {
            _uiItemsList.Add(new UiItem {itemImage = FirstItemImage, itemCountText = FirstItemCount, itemNumber = 1});
            _uiItemsList.Add(new UiItem {itemImage = SecondItemImage, itemCountText = SecondItemCount, itemNumber = 2});
            _uiItemsList.Add(new UiItem {itemImage = ThirdItemImage, itemCountText = ThirdItemCount, itemNumber = 3});
            
            _gameGui = FindObjectOfType<GameGui>(); 
        }

        public void TurnOn(Dictionary<string, int> items, Chest chest) {
            _activeChest = chest;
            foreach (var item in items) {                
                AddItem(item.Key, item.Value.ToString());
            }            
            CanvasObject.enabled = true;            
        }

        public void UpdateGui(Dictionary<string, int> items) {
            foreach (var uiItem in _uiItemsList) {
                uiItem.itemImage.enabled = false;
                uiItem.itemCountText.enabled = false;
            }
            
            foreach (var item in items) {                               
                AddItem(item.Key, item.Value.ToString());
            }
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
        
        private UiItem GetItem(int number) {
            return (from uiItem in _uiItemsList where uiItem.itemNumber == number select uiItem).FirstOrDefault();
        }
        
        private UiItem GetItem(Image image) {
            return _uiItemsList.FirstOrDefault(uiItem => uiItem.itemImage == image);
        }       
        
        public void UiItemWasDragedToPlayer(UiItem item, Image sprite) {
            item.itemImage = sprite;
            item.itemCountText.enabled = false;
            _activeChest.ItemPicked(item.itemName);
        }
        
        public void UiItemWasDragedToChest(UiItem item, Image sprite) {
            item.itemImage = sprite;
            item.itemCountText.enabled = false;
            _activeChest.ItemPlaced(item.itemName, int.Parse(item.itemCountText.text));
        }

        public UiItem GetRelatedUiItem(Image image) {
            return GetItem(image);
        }
        
        public void OnSheetChange(DragAndDropCell.DropDescriptor desc) { // Drag to chest            
            var sourceSprite = desc.sourceCell.GetItem().GetComponent<Image>();
            var destinationSprite = desc.item.GetComponent<Image>();
            
            Debug.Log(sourceSprite.sprite);
            Debug.Log(destinationSprite.sprite);
            
            destinationSprite.enabled = false;
            
            var pickedItem = _gameGui.GetRelatedUiItem(destinationSprite);
            
            var parent = desc.destinationCell.GetItem().transform.parent.gameObject;

            UiItem item = null;
            
            switch (parent.name) {
                case "FirstItem": {
                    item = GetItem(1);                                         
                    break;
                }
                case "SecondItem": {
                    item = GetItem(2);                    
                    break;
                }
                case "ThirdItem": {
                    item = GetItem(3);                    
                    break;
                }
            }

            if (item != null) {
                item.itemImage = destinationSprite;
                item.itemImage.transform.rotation = Quaternion.Euler(0, 0, 0);                
            }
            UiItemWasDragedToChest(pickedItem, sourceSprite);
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