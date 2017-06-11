using Controls.Bosses;
using Loot;
using UnityEngine;
using UnityEngine.Networking;


namespace CharacterControllers.Enemies.Bosses {
	public class ServerBossShieldShieldedController : ServerCharacterController {

		public ServerBossShieldedController Parent { get; set; }

		private void Start() {
            IsNpc = true;
			
		}

        public override void TakeDamage(int amount, float timeOfDeath = 2f) {
            if (!isServer)
				return;
			Parent.TakeDamage(999, gameObject);
        }

	}

}

