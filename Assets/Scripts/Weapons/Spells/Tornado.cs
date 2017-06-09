using CharacterControllers;
using UnityEngine;

namespace Weapons.Spells {
    public class Tornado : Weapon {               
        
        void OnCollisionEnter(Collision other) {
            var go = other.gameObject;                        
            if (go.CompareTag("Enemy")|| go.CompareTag("Player")) {
                go.GetComponent<ServerCharacterController>().TakeDamage(100, 4.5f);
                var rigidBody = go.GetComponent<Rigidbody>();
                if (rigidBody) {
                    rigidBody.velocity = go.transform.up * 20;
                }                
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