using System.Collections.Generic;
using System.Linq;
using App.Eventhub;
using Loot;
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

        private DragAndDropCell.DropDescriptor _previusDropDescriptor;
        
        internal class UiItem {
            public Image itemImage;
            public Text itemCountText;
            public string itemName;
            public int itemNumber;

            public override string ToString() {
                return  $"{itemName} with {itemCountText.text} is {itemNumber}";
            }
        }

        private void Awake() {
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
            return _uiItemsList.FirstOrDefault(item => !item.itemImage.enabled);
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

        public void OnItemPlace(DragAndDropCell.DropDescriptor desc) {
            if (desc.destinationCell == _previusDropDescriptor.sourceCell) return;

            _previusDropDescriptor = desc;
            
            if (desc.sourceCell.cellType == DragAndDropCell.CellType.DragOnly) { // drag From Chest
                Debug.Log(123);
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