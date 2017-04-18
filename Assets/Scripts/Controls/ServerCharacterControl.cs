using UnityEngine;
using UnityEngine.Networking;

namespace Controls {
    [NetworkSettings(channel = 1, sendInterval = 1f)]
    public class ServerCharacterControl : NetworkBehaviour {
        [SyncVar]
        public int CurrentHealth = 100;
        public bool destroyOnDeath;
        public bool isNPC;

        public void TakeDamage(int amount) {
            if (!isServer) return;

            CurrentHealth -= amount;
            if (CurrentHealth > 0) return;
            CurrentHealth = 0;
            if (!destroyOnDeath) return;
            if (!isNPC) {
                App.NetworkEventHub.Instance.RpcPublishEvent("PlayerDied", gameObject.name);
                Destroy(gameObject);
            }
            if (isNPC) {
                GetComponent<EnemyController>().Die();
                Destroy(gameObject, 2f); // time after enemy will be destroyed. Maybe replace to fadeout.
            }
        }

        private void Update() {
            if (transform.position.y < -2.5) {
               TakeDamage(100);
            }
        }

    }
}