
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

		private void Awake() {
			if (!Instance) {
				Instance = this;
			} else {
				Destroy(gameObject);
			}
			DontDestroyOnLoad(gameObject);
		}

		public void CreateLoot(Vector3 where) {
			if (Random.Range(0, 101) <= chanceOfSpawnLoot) {
				var instantiated = Instantiate(lootObject, where, Quaternion.identity);
				NetworkServer.Spawn(instantiated);
			}
		}

	}
}
