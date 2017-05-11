using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class GameGui: MonoBehaviour {
        [SerializeField] private RawImage FirstItemImage;
        [SerializeField] private Text FirstItemCount;
        [SerializeField] private RawImage SecondItemImage;
        [SerializeField] private Text SecondItemCount;
        [SerializeField] private RawImage ThirdItemImage;
        [SerializeField] private Text ThirdItemCount;

        public void EnableFirstItem(string count) {
//            FirstItemImage.texture = texture;
            FirstItemCount.text = count;
            FirstItemImage.enabled = true;
            FirstItemCount.enabled = true;
        }

        public void ModifyFirstItemCount(string count) {
            FirstItemCount.text = count;
        }

        public void DisableFirstItem() {
            FirstItemImage.enabled = false;
            FirstItemCount.enabled = false;
        }

    }
}