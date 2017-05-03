using UnityEngine;

namespace MazeBuilder.Walls {
    public class WallsTorches : MonoBehaviour {
        [Range(0, 100f)] public float TorchesDisableChance = 50;

        public void Start() {
            if (Random.Range(0, 101) <= TorchesDisableChance) {
                gameObject.SetActive(false);
            }
        }
    }
}