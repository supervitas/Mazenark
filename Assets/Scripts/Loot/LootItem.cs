using Controls;
using UnityEngine;

namespace Loot {
	class LootItem : MonoBehaviour	{
//		public void OnTriggerEnter(Collider other) {
//			var go = other.gameObject;
//			if (go.CompareTag("Player")) {
//				// player.addFireball();
////				Destroy(gameObject.transform.parent.gameObject);
//
//				Destroy(gameObject);
//			}
//		}

		public void OnTriggerExit(Collider other) {
			// remove ui
		}

		public void OnDestroy() {
			// stop drawing ui
		}
	}
}
