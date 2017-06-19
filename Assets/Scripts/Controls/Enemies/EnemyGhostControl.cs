using CharacterControllers;
using UnityEngine;

namespace Controls.Enemies {   
    public class EnemyGhostControl : BasicEnemyControl {
              
        protected override void Fire(Vector3 direction) {
            RaycastHit hit;
            var pos = transform.position;

            pos.y = 1f;

            if (Physics.Raycast(pos, direction, out hit, 4.5f)) {
                var go = hit.transform.gameObject;
                if (go.CompareTag("Player")) {
                    go.GetComponent<ServerCharacterController>().TakeDamage(100);                    
                }
            }
        }
    }
}
