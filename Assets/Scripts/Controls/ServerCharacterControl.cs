using App;
using Loot;
using UnityEngine;
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
                InvokeRepeating("PlayerUpdate", 0, 1f);
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
                Destroy(gameObject, 2f); // time after enemy will be destroyed. Maybe replace to fadeout
                var loot = FindObjectOfType<LootManager>().GetLoot(); // Находим первый объект LootManager в сцене.
                ServerSpawner.Instance.ServerSpawn(loot, transform.position, Quaternion.identity); // Cпавним объект. И рассылаем то, что мы отспавнили на все клиенты.
                // Также к геймобжекту, который собираемся инстансить нужно добавить Network Identity, и зарегистрировать в LobbyManager(См скриншот в вк).
            }
        }

        private void PlayerUpdate() {
            if (transform.position.y < -2.5) {
                TakeDamage(100);
            }
        }
    }
}