using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyTopPanel : MonoBehaviour
    {
        public bool isInGame = false;

        protected bool isDisplayed = true;
        protected Image panelImage;

        public GameObject BackToGameImage;

        void Start() {
            panelImage = GetComponent<Image>();
            
        }


        void Update() {
            if (!isInGame) {
                BackToGameImage.SetActive(false);
                return;
            }
            if (isDisplayed) {
                BackToGameImage.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ToggleVisibility(!isDisplayed);
            }
        }

        public void CloseMenu() {
            ToggleVisibility(false);
        }

        public void ToggleVisibility(bool visible)
        {
            isDisplayed = visible;
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(isDisplayed);
            }

            if (panelImage != null)
            {
                panelImage.enabled = isDisplayed;
            }
        }
    }
}