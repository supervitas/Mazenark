﻿using Controls;
using UnityEngine;

namespace Weapons.Spells {
    public class Fireball : Weapon {
        private void Start() {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
                if (player.GetComponent<PlayerControl>().isLocalPlayer) {
                    Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>()); // ignore self colide
                }
            }
        }

        void OnCollisionEnter(Collision other) {
            var go = other.gameObject;
            go.SendMessage("TakeDamage", 100.0F, SendMessageOptions.DontRequireReceiver); // execute function on colided object.
            Destroy(this);
        }

        public override void OnDestroy() {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(gameObject, 1f);
        }
    }
}