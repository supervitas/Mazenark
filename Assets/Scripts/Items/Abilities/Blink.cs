using UnityEngine;

namespace Items.Abilities {
    public class Blink : MonoBehaviour {
        
        public void BlinkPlayer(Vector3 direction, GameObject player) {
            player.GetComponent<Rigidbody>().AddForce(direction * 1000, ForceMode.Impulse);
        }
    }
}