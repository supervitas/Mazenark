using Controls;
using UnityEngine;

namespace Weapons.Spells {
    public class Tornado : Weapon {       
        

        void OnCollisionEnter(Collision other) {
            var go = other.gameObject;
            go.SendMessage("TakeDamage", 100.0F, SendMessageOptions.DontRequireReceiver); // execute function on colided object.
//            Destroy(this);
        }

        public void OnDestroy() {        
            Destroy(gameObject, 1f);
        }


        public override void Fire() {
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * 20;
        }
    }
}