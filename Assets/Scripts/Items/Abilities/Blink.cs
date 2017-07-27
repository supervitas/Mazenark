using UnityEngine;

namespace Items.Abilities {
    public class Blink : MonoBehaviour {
        
        public void BlinkPlayer(Vector3 direction) {
            gameObject.transform.position = direction;
        }
    }
}