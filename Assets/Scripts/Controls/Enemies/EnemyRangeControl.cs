using Items;
using UnityEngine;
using UnityEngine.Networking;

namespace Controls.Enemies {    
    public class EnemyRangeControl : BasicEnemyControl {
        [SerializeField]
        [Range(0, 10)] 
        private float _faultOfFire = 5f;       

        protected override void Fire(Vector3 direction) {
            var pos = transform.position;
            pos.y += 1.5f;
            
            direction.y += 2f;
            direction.z += Random.Range(-_faultOfFire, _faultOfFire);
            direction.x += Random.Range(-_faultOfFire, _faultOfFire);
            
            var activeItem = Instantiate(Weapon, pos, Quaternion.identity);
            var weapon = activeItem.GetComponent<Weapon>();
            activeItem.transform.LookAt(direction);
            weapon.Fire();
            NetworkServer.Spawn(activeItem);
            Destroy(weapon, 10.0f);
        }
    }
}
