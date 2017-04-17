
using App.EventSystem;
using Lobby;
using UnityEngine;
using UnityEngine.Networking;

namespace Controls {
    [NetworkSettings(channel = 0, sendInterval = 1f)]
    public class ServerCharacterControl : NetworkBehaviour {
        [SyncVar]
        public int CurrentHealth = 100;
        public bool destroyOnDeath;

        public void TakeDamage(int amount) {
            if (!isServer) return;

            CurrentHealth -= amount;
            if (CurrentHealth <= 0) {
                CurrentHealth = 0;
                if (destroyOnDeath) {
                    App.NetworkEventHub.Instance.RpcPublishEvent("PlayerDied", gameObject.name);
                    Destroy(gameObject);
                }
            }

        }

        private void Update() {
            if (transform.position.y < -0.5) {
               TakeDamage(100);
            }
        }

    }
}