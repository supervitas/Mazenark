using App.EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class BiomeChanged : MonoBehaviour{
        public Canvas CanvasObject;
        // Use this for initialization
        void Start () {
            App.AppManager.Instance.EventHub.Subscribe("maze:biomeChanged", OnLocationChanged, this);
        }

        private void OnDestroy() {
            App.AppManager.Instance.EventHub.UnsubscribeFromAll(this);
        }

        private void OnLocationChanged(object sender, EventArguments args) {
            if(args.BiomeName == "Spawn Biome" || args.BiomeName == "Safehouse Biome") return;

            CanvasObject.enabled = true;
            var t = transform.GetChild(0).GetComponent<Text>();
            t.text = string.Format("{0}", args.BiomeName);
            Invoke("TurnOffCanvas", 2.25f);
        }

        private void TurnOffCanvas() {
            CanvasObject.enabled = false;
        }

    }
}