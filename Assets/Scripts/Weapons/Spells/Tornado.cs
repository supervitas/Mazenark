using Controls;
using UnityEngine;

namespace Weapons.Spells {
    public class Tornado : Weapon {       

        private void Start() {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
                if (player.GetComponent<PlayerControl>().isLocalPlayer) {
                    Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>()); // ignore self colide
                }
            }
        }

        void OnCollisionEnter(Collision other) {
            var go = other.gameObject;
            go.SendMessage("TakeDamage", 100.0F, SendMessageOptions.DontRequireReceiver); // execute function on colided object.
            Destroy(this);
        }

        public override void OnDestroy() {        
            Destroy(gameObject, 1f);
        }
    
        
    }
}