using UnityEngine.Networking;

namespace Controls {
    [NetworkSettings(channel = 0, sendInterval = 0.2f)]
    public class ServerCharacterControl : NetworkBehaviour {

        [SyncVar]
        public int CurrentHealth = 100;
        public bool destroyOnDeath;
        public bool isNPC;

        private void Start() {
            if (!isNPC) {
                InvokeRepeating("PlayerUpdate", 0, 1.5f);
            }
        }

        private void OnDestroy() {
            if (!isNPC) {
                CancelInvoke("PlayerUpdate");
            }
        }

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
                GetComponent<EnemyController>().Die(); // Play animation
                Destroy(gameObject, 3.5f); // time after enemy will be destroyed. Maybe replace to fadeout
            }
        }

        private void PlayerUpdate() {
            if (transform.position.y < -5.5) {
                TakeDamage(100);
            }
        }
    }
}