﻿using App;
using Controls;
using Lobby;
using Loot;
using UnityEngine.Networking;

namespace CharacterControllers {
    [NetworkSettings(channel = 0, sendInterval = 0.2f)]
    public class ServerCharacterController : NetworkBehaviour {
        
        public int CurrentHealth = 100;

        protected bool IsNpc;        
         

        public virtual void TakeDamage(int amount, float timeOfDeath = 2f) {
            if (!isServer) return;

            CurrentHealth -= amount;
            if (CurrentHealth > 0) return;
            CurrentHealth = 0;            

            if (!IsNpc) {
                FindObjectOfType<LobbyGameManager>().OnGameover(gameObject.name);

                NetworkEventHub.Instance.RpcPublishEvent("PlayerDied", gameObject.name);
                Destroy(gameObject);
            }
            if (IsNpc) {
                GetComponent<EnemyControl>().Die(); // Play animation
                Destroy(gameObject, timeOfDeath); // time after enemy will be destroyed. Maybe replace to fadeout
                    var pos = transform.position;
                    pos.y = 1.5f;
                    FindObjectOfType<LootManager>().CreateLoot(pos);
                }
        }

    }
}