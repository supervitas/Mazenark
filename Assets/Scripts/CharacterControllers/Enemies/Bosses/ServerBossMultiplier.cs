using UnityEngine;


namespace CharacterControllers.Enemies.Bosses {    
    public class ServerBossMultiplier : ServerCharacterController {
        private void Start() {
            IsNpc = true;                        
        }

        public override void TakeDamage(int amount, float timeOfDeath) {
            Debug.Log(amount);
        }
    }
}
