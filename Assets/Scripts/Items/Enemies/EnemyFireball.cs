using CharacterControllers;
using UnityEngine;

namespace Items.Enemies {
    public class EnemyFireball : Weapon {
        
        protected override void Start() {}

        void OnCollisionEnter(Collision other) {
            var go = other.gameObject;            
            if (go.CompareTag("Player")) {                
                go.GetComponent<ServerCharacterController>().TakeDamage(100, 3.5f);   
            }            
            Destroy(this);
        }

        public void OnDestroy() {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(gameObject, 1f);
        }

        public override void Fire() {            
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * 8;
        }
    }
}