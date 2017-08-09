using UnityEngine;

namespace Items.Abilities {
    public class Blink : MonoBehaviour {
        
        public void BlinkPlayer(Vector3 direction, GameObject player) {
            player.GetComponent<Rigidbody>().AddForce(direction * 2000, ForceMode.Impulse);
        }      
    }
}