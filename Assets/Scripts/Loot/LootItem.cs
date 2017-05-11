using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Loot {
	class LootItem : MonoBehaviour	{
		public void OnTriggerEnter(Collider other) {
			if (other.gameObject.CompareTag("Player")) {
				var player = other.gameObject;
				// player.addFireball();
				Destroy(gameObject.transform.parent.gameObject);
				Debug.Log("Player picked up teh fierbawl!");
			}
		}

		public void OnTriggerExit(Collider other) {
			// remove ui
		}

		public void OnDestroy() {
			// stop drawing ui
		}
	}
}
