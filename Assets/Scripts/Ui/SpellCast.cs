using App.EventSystem;
using ProgressBar;
using UnityEngine;

namespace Ui {
    public class SpellCast : MonoBehaviour {
        public Canvas CanvasObject;
        // Use this for initialization
        private Canvas _canvas;

        private ProgressBarBehaviour _progressBar;
        void Start () {
            _canvas = CanvasObject.GetComponent<Canvas>();
            _progressBar = transform.GetChild(0).GetComponent<ProgressBarBehaviour>();
        }

        public void SetProgress(float progress) {
            _canvas.enabled = true;
            _progressBar.Value = progress;
            _progressBar.SetFillerSizeAsPercentage(progress);
        }

        public void Reset() {
            _progressBar.Value = 0;
            _canvas.enabled = false;
        }

    }
}