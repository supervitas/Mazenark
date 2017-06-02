using Controls;
using UnityEngine;

namespace Weapons.Spells {
    public class Fireball : Weapon {

        void OnCollisionEnter(Collision other) {
            var go = other.gameObject;
            go.SendMessage("TakeDamage", 100.0F, SendMessageOptions.DontRequireReceiver); // execute function on colided object.
            Destroy(this);
        }

        public void OnDestroy() {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(gameObject, 1f);
        }

        public override void Fire() {            
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * 15;
        }
    }
}