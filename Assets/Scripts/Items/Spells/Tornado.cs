using CharacterControllers;
using UnityEngine;

namespace Items.Spells {
    public class Tornado : Weapon {               
        
        private void OnCollisionEnter(Collision other) {
            var go = other.gameObject;                        
            if (go.CompareTag("Enemy") || go.CompareTag("Player")) {
                go.GetComponent<ServerCharacterController>().TakeDamage(100, 2f, PlayerCasted);                            
            }            
        }

        public void OnDestroy() {        
            Destroy(gameObject, 1f);
        }

        public override void Fire() {
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * 13;
        }
    }
}