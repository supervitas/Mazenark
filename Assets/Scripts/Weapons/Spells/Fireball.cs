using UnityEngine;

namespace Weapons.Spells {
    public class Fireball : MonoBehaviour {

        [Range(0, 30f)] public float CastTime = 1.2f;

        void OnCollisionEnter(Collision other) {
            other.gameObject.SendMessage("TakeDamage", 100.0F, SendMessageOptions.DontRequireReceiver); // execute function on colided object.
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(gameObject, 1f);
        }
    }
}