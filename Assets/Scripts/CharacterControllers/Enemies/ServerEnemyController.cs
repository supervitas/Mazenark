using Controls;
using Loot;
using UnityEngine;

namespace CharacterControllers.Enemies {
    public class ServerEnemyController : ServerCharacterController {
        private void Start() {
            if (!isServer) return;
            
            IsNpc = true;                 
        }

        public override void TakeDamage(int amount, float timeOfDeath = 2) {
            if (!isServer) return;
            
            var control = GetComponent<BasicEnemyControl>();

            GetComponent<Collider>().enabled = false;
            
            if (!control.IsAlive) return;
            
            control.Die(timeOfDeath);
            
            Destroy(gameObject, timeOfDeath);
            
            var pos = transform.position;
            pos.y = 1.5f;
            
            FindObjectOfType<LootManager>().CreateLoot(pos);            
        }
    }
}