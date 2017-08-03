using Items;
using UnityEngine;
using UnityEngine.Networking;

namespace Controls.Enemies {    
    public class EnemyRangeControl : BasicEnemyControl {

        protected override void Fire(Vector3 direction) {
            var pos = transform.position;
            pos.y += 1.5f;
            
            direction.y += 2f;
            direction.z += Random.Range(-2, 3);
            direction.x += Random.Range(-2, 3);
            
            var activeItem = Instantiate(Weapon, pos, Quaternion.identity);
            var weapon = activeItem.GetComponent<Weapon>();
            activeItem.transform.LookAt(direction);
            weapon.Fire();
            NetworkServer.Spawn(activeItem);
            Destroy(weapon, 10.0f);
        }
    }
}
