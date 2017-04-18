using UnityEngine;

namespace Weapons.Spells {
    public class Fireball : MonoBehaviour {

        void OnCollisionEnter(Collision other) {
            other.gameObject.SendMessage("TakeDamage", 100.0F, SendMessageOptions.DontRequireReceiver); // execute function on colided object.
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(gameObject, 1f);
        }

        void Update() {
            transform.Rotate(Vector3.right * Time.deltaTime * 60);
        }
    }
}