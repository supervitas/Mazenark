using UnityEngine.Networking;

namespace CharacterControllers {
    [NetworkSettings(channel = 0, sendInterval = 0.2f)]
    public abstract class ServerCharacterController : NetworkBehaviour {
        
        protected int CurrentHealth = 100;
        protected bool IsNpc;

        public abstract void TakeDamage(int amount, float timeOfDeath = 2f, string whoCasted = "Enemy");
    }
}