using CharacterControllers;
using UnityEngine;

namespace Controls.Enemies {    
    public class EnemySlimeControl : BasicEnemyControl {

        protected override void Fire(Vector3 direction) {
            RaycastHit hit;
            var pos = transform.position;
            pos.y = 2.5f;
            Debug.DrawLine(pos, direction, Color.red);
            if (Physics.Raycast(pos, direction, out hit, 6f)) {               
                var go = hit.transform.gameObject;
                Debug.Log(go.tag);
                if (go.CompareTag("Player")) {
                    go.GetComponent<ServerCharacterController>().TakeDamage(100);                    
                }
            }
        }
    }
}
