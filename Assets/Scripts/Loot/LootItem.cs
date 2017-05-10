using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Loot {
	class LootItem : MonoBehaviour	{
		public void OnCollisionEnter(Collision collision) {
			// if player => draw ui for pick up
		}

		public void OnCollisionExit(Collision collision) {
			// stop drawing ui
		}

		public void OnDestroy() {
			// stop drawing ui
		}
	}
}
