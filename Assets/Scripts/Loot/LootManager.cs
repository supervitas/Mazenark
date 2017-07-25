using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Loot {
	public class LootManager : NetworkBehaviour {
		private static LootManager Instance;
		private readonly CollectionRandom _loots = new CollectionRandom();

		[SerializeField]
		[Range(0, 100f)] private float chanceOfSpawnLoot = 30f;

		[SerializeField]
		private GameObject[] loot;		

		private void Start() {
			foreach (var lootItem in loot) {
				_loots.Add(lootItem, typeof(GameObject));
			}		
		}

		public void CreateLoot(Vector3 where, float chance = float.NaN ) {

			if (float.IsNaN(chance)) {
				chance = Random.Range(0, 100);
			}
						
			if (chance >= chanceOfSpawnLoot) {
				var instantiated = Instantiate((GameObject) _loots.GetRandom(typeof(GameObject)), where, Quaternion.identity);
				NetworkServer.Spawn(instantiated);
			}
		}
	}
}
