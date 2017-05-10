using MazeBuilder.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Loot {
	class LootManager : MonoBehaviour {
		private CollectionRandom loots = new CollectionRandom();

		[SerializeField]
		public GameObject lootObject;

		public void SpawnLoot(Vector3 where) {
			Instantiate(lootObject, where, Quaternion.identity); // Это отспавнит только сервер, клиенты это не увидят.
		}

		public GameObject GetLoot() { // Возвращаем объект который нужно отспавнить..
			return lootObject;
		}

	}
}
