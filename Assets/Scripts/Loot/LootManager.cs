
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace Loot {
	public class LootManager : NetworkBehaviour {
		private static LootManager Instance;
		private CollectionRandom loots = new CollectionRandom();

		[SerializeField]
		[Range(0, 100f)] private float chanceOfSpawnLoot = 30f;

		[SerializeField]
		private GameObject lootObject;

		public void CreateLoot(Vector3 where, float chanse = 0.1f ) {
			if (chanse == 0.1f) {
				chanse = Random.Range(0, 101);
			}
			if (chanse <= chanceOfSpawnLoot) {
				var instantiated = Instantiate(lootObject, where, Quaternion.identity);
				NetworkServer.Spawn(instantiated);
			}
		}

	}
}
