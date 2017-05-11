using MazeBuilder.Utility;
using UnityEngine;

namespace Loot {
	class LootManager : MonoBehaviour {
		private CollectionRandom loots = new CollectionRandom();

		[SerializeField]
		private GameObject lootObject;

		public GameObject GetLoot() {
			return lootObject;
		}

	}
}
