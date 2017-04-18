using UnityEngine;

namespace Weapons.Spells {
    public class Fireball : MonoBehaviour {

        private void Start() {
        }

        void OnCollisionEnter(Collision other) {
            Debug.Log(other.gameObject.name);
        }
    }
}