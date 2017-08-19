using ProgressBar;
using UnityEngine;

namespace Ui {
    public class SpellCast : MonoBehaviour {
        public Canvas CanvasObject;        

        private ProgressBarBehaviour _progressBar;

        private void Start () {
            _progressBar = transform.GetChild(0).GetComponent<ProgressBarBehaviour>();
        }

        public void SetProgress(float progress) {
            CanvasObject.enabled = true;
            _progressBar.Value = progress;
            _progressBar.SetFillerSizeAsPercentage(progress);
        }

        public void Reset() {
            _progressBar.Value = 0;
            CanvasObject.enabled = false;
        }
    }
}