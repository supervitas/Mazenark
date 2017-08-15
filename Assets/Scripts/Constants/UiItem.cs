using UnityEngine.UI;

namespace Constants {
    public class UiItem {
        public Image itemImage;
        public Text itemCountText;
        public string itemName;
        public int itemNumber;        

        public override string ToString() {
            return  $"{itemName} with {itemCountText.text} is {itemNumber}";
        }
    }
}