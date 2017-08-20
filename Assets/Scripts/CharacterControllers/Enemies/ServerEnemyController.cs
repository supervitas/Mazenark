using Controls;
using GameEnv.GameEffects;
using GameSystems;
using Loot;
using UnityEngine;
using UnityEngine.Networking;

namespace CharacterControllers.Enemies {
    public class ServerEnemyController : ServerCharacterController {
        private void Start() {
            if (!isServer) return;
            
            IsNpc = true;                 
        }

        public override void TakeDamage(int amount, float timeOfDeath = 2, string whoCasted = "Enemy") {
            if (!isServer) return;

            FindObjectOfType<GameManager>().PlayerKilledEnemy(whoCasted);
            
            var control = GetComponent<BasicEnemyControl>();

            GetComponent<Collider>().enabled = false;
            
            if (!control.IsAlive) return;
            
            control.Die(timeOfDeath);
            
            Destroy(gameObject, timeOfDeath);
            
            var pos = transform.position;
            pos.y = 1.5f;

            RpcStartDisolve(timeOfDeath / 2);
            FindObjectOfType<LootManager>().CreateLoot(pos);            
        }
        
        [ClientRpc]
        protected void RpcStartDisolve(float waitTime) {
           Invoke(nameof(Disolve), waitTime);                                   
        }

        private void Disolve() {
            GetComponent<Disolve>().BeginDisolve(); 
        }
    }
}