using CharacterControllers;
using UnityEngine;

namespace Controls.Enemies {   
    public class EnemyMeleeControl : BasicEnemyControl {
        public override void OnStartServer() {
            base.OnStartServer();
            RangeOfAttack = 6f;
        }

        protected override void Fire(Vector3 direction) {
            var pos = transform.position;
            var dir = direction;
            pos.y += 2.5f;           
            dir.y += 1.5f;
              
            if (Physics.Linecast(pos, direction, out RaycastHit)) {                
                var go = RaycastHit.transform.gameObject;
            
                if (go.CompareTag("Player")) {           
                    go.GetComponent<ServerCharacterController>().TakeDamage(100);                    
                }
            }
        }
    }
}
