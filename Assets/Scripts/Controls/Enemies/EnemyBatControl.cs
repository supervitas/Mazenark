using CharacterControllers;
using UnityEngine;

namespace Controls.Enemies {    
    public class EnemyBatControl : BasicEnemyControl {


        protected override void Fire(Vector3 direction) {
            RaycastHit hit;
            var pos = transform.position;

                        
            Debug.DrawRay(pos, direction, Color.gray, 4.5f);
            if (Physics.Raycast(pos, direction, out hit, 4.5f)) {
                var go = hit.transform.gameObject;
                if (go.CompareTag("Player")) {
                    go.GetComponent<ServerCharacterController>().TakeDamage(100);                    
                }
            }
        }
    }
}
