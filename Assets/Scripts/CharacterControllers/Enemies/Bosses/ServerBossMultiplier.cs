﻿using Controls;
using Loot;
using UnityEngine;


namespace CharacterControllers.Enemies.Bosses {    
    public class ServerBossMultiplier : ServerCharacterController {
        private int hitCounter = 3;
        private int hited = 0;
        private void Start() {
            IsNpc = true;                        
        }

        public override void TakeDamage(int amount, float timeOfDeath) {
            if (!isServer) return;
            hited++;
            if (hited >= hitCounter) {
                GetComponent<EnemyControl>().Die(); // Play animation
                Destroy(gameObject, timeOfDeath); // time after enemy will be destroyed. Maybe replace to fadeout
                var pos = transform.position;
                pos.y = 1.5f;
                FindObjectOfType<LootManager>().CreateLoot(pos, 100f);
            }
        }
    }
}
