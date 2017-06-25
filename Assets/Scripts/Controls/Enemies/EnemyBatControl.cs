using CharacterControllers;
using UnityEngine;

namespace Controls.Enemies {    
    public class EnemyBatControl : BasicEnemyControl {        

//        protected override void Fire(Vector3 direction) {
//            RaycastHit hit;
//            var pos = transform.position;
//            pos.y = 2.5f;                      
//            if (Physics.Raycast(pos, direction, out hit, 6f)) {                
//                var go = hit.transform.gameObject;
//                if (go.CompareTag("Player")) {
//                    go.GetComponent<ServerCharacterController>().TakeDamage(100);                    
//                }
//            }
//        }
    }
}
