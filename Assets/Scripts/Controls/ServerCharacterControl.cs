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
        private CharacterControl _characterControl;
        private LootManager _lootManager;

        private void Start() {
            if (isServer) {
                if (!isNPC) {
                    InvokeRepeating("PlayerUpdate", 0, 0.5f);
                    _characterControl = GetComponent<CharacterControl>();
                    _characterControl.SetFireballsOnServer(5);
                    _characterControl.TargetSetFireballs(connectionToClient, 5);
                }
                _lootManager = FindObjectOfType<LootManager>();
            }
        }

        private void OnDestroy() {
            if (!isNPC) {
                CancelInvoke("PlayerUpdate");
            }
        }

        private void OnTriggerEnter(Collider other) {
            if(!isServer) return;

            if (isNPC) return;
            var go = other.gameObject;
            if (go.CompareTag("Pickable")) {
                _characterControl.SetFireballsOnServer(1);
                _characterControl.TargetAddFireballs(connectionToClient, 1);
                Destroy(go);
            }
        }

        public void TakeDamage(int amount) {
            if (!isServer) return;

            CurrentHealth -= amount;
            if (CurrentHealth > 0) return;
            CurrentHealth = 0;
            if (!destroyOnDeath) return;
            if (!isNPC) {
                NetworkEventHub.Instance.RpcPublishEvent("PlayerDied", gameObject.name);
                Destroy(gameObject);
            }
            if (isNPC) {
                GetComponent<EnemyController>().Die(); // Play animation
                Destroy(gameObject, 2f); // time after enemy will be destroyed. Maybe replace to fadeout

                if (Random.Range(0, 3) == 1) {
                    var loot = _lootManager.GetLoot();
                    var pos = transform.position;
                    pos.y = 1.5f;
                    ServerSpawner.Instance.ServerSpawn(loot, pos, Quaternion.identity);
                }
            }
        }

        private void PlayerUpdate() {
            if (transform.position.y < -2.5) {
                TakeDamage(100);
            }
        }
    }
}