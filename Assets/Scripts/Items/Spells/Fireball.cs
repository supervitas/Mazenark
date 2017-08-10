using CharacterControllers;
using UnityEngine;

namespace Items.Spells {
    public class Fireball : Weapon {

        private void OnCollisionEnter(Collision other) {
            var go = other.gameObject;            
            if (go.CompareTag("Player") || go.CompareTag("Enemy")) {              
                go.GetComponent<ServerCharacterController>().TakeDamage(100, 2.5f);   
            }            
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