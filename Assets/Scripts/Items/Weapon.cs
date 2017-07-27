using Controls;
using UnityEngine;

namespace Items {
    public abstract class Weapon : MonoBehaviour {
        [SerializeField]
        [Range(0, 10f)] protected float CastTime = 2.3f;
        
        protected virtual void Start() {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
                if (player.GetComponent<PlayerControl>().isLocalPlayer) {
                    Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>()); // ignore self colide
                }
            }
        }

        public abstract void Fire();

        public float GetCastTime() {
            return CastTime;
        }
    }
}