using Controls;
using UnityEngine;

namespace Weapons.Spells {
    public class Fireball : MonoBehaviour {

        [Range(0, 10f)] public float CastTime = 1.2f;

        private void Start() {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
                if (player.GetComponent<CharacterControl>().isLocalPlayer) {
                    Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>()); // ignore self colide
                }
            }
        }

        void OnCollisionEnter(Collision other) {
            var go = other.gameObject;
            go.SendMessage("TakeDamage", 100.0F, SendMessageOptions.DontRequireReceiver); // execute function on colided object.
            Destroy(this);
        }

        private void OnDestroy() {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(gameObject, 1f);
        }
    }
}