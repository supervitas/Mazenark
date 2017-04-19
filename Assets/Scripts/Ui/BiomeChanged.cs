using App.EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class BiomeChanged : MonoBehaviour{
        public Canvas CanvasObject;
        // Use this for initialization
        private Canvas _canvas;
        void Start () {
            App.AppManager.Instance.EventHub.Subscribe("maze:biomeChanged", OnLocationChanged, this);
            _canvas = CanvasObject.GetComponent<Canvas>();
        }

        private void OnLocationChanged(object sender, EventArguments args) {
            if(args.BiomeName == "Spawn Biome" || args.BiomeName == "Safehouse Biome") return;

            _canvas.enabled = true;
            var t = transform.GetChild(0).GetComponent<Text>();
            t.text = string.Format("{0}", args.BiomeName);
            Invoke("TurnOffCanvas", 2.25f);
        }

        private void TurnOffCanvas() {
            _canvas.enabled = false;
        }

    }
}