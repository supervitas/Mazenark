using Controls;
using Loot;

namespace CharacterControllers.Enemies {
    public class ServerEnemyController : ServerCharacterController {
        private void Start() {
            IsNpc = true;                 
        }

        public override void TakeDamage(int amount, float timeOfDeath = 2) {
            var control = GetComponent<EnemyControl>();
            if (!control.IsAlive()) return;                
            control.Die();
            Destroy(gameObject, timeOfDeath); // time after enemy will be destroyed. Maybe replace to fadeout
            var pos = transform.position;
            pos.y = 1.5f;
            FindObjectOfType<LootManager>().CreateLoot(pos);
        }
    }
}