using CharacterControllers;
using UnityEngine;

namespace Items.Spells {
    public class Lightning: Weapon {

        protected override void Start() {}
        
        public override void Fire() {
            RaycastHit hit;
            Vector3 fwd = gameObject.transform.TransformDirection(Vector3.forward);            

            if (Physics.Raycast(transform.position, fwd, out hit, 25f)) {
                var go = hit.transform.gameObject;                        
                if (go.CompareTag("Enemy") || go.CompareTag("Player")) {
                    go.GetComponent<ServerCharacterController>().TakeDamage(100, 2f, PlayerCasted);                            
                }
            }
            Destroy(gameObject, 1.2f);
        }
    }
}