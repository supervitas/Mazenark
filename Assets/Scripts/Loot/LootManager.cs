using System;
using System.Collections.Generic;
using App;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Loot {
	public class LootManager : NetworkBehaviour {
		private static LootManager Instance;
		private readonly CollectionRandom _loots = new CollectionRandom();

		[SerializeField]
		[Range(0, 100f)] private float _chanceOfSpawnLoot = 30f;
		
		[SerializeField]
		[Range(0, 100f)] private float _chanceOfSpawnChest = 50f;

		[SerializeField]
		private GameObject[] _loot;
		
		[SerializeField]
		private GameObject _chest;

		private Maze _maze;

		public override void OnStartServer() {
			foreach (var lootItem in _loot) {
				_loots.Add(lootItem, typeof(GameObject));
			}
			_maze = AppManager.Instance.MazeInstance.Maze;
			SpawnChests();
		}

		public void CreateLoot(Vector3 where, float chance = float.NaN ) {

			if (float.IsNaN(chance)) {
				chance = Random.Range(0, 100);
			}
						
			if (chance >= _chanceOfSpawnLoot) {
				var instantiated = Instantiate((GameObject) _loots.GetRandom(typeof(GameObject)), where, Quaternion.identity);
				NetworkServer.Spawn(instantiated);
			}
		}

		private bool CanSpawnTop(int i, int j) {
			try {
				if (_maze[i, j].Type == Tile.Variant.Empty &&
				    _maze[i - 1, j].Type == Tile.Variant.Wall &&
				    _maze[i + 1, j].Type == Tile.Variant.Wall &&
				    _maze[i, j + 1].Type == Tile.Variant.Wall) {
					return true;
				}
			}
			catch (IndexOutOfRangeException) { }		
			return false;
		}

		private bool CanSpawnBottom(int i, int j) {
			try {
				if (_maze[i, j].Type == Tile.Variant.Empty &&
				    _maze[i - 1, j].Type == Tile.Variant.Wall &&
				    _maze[i + 1, j].Type == Tile.Variant.Wall &&
				    _maze[i, j - 1].Type == Tile.Variant.Wall) {
					return true;
				}
			}
			catch (IndexOutOfRangeException) {}		
			return false;
		}

		private bool CanSpawnLeft(int i, int j) {
			try {
				if (_maze[i, j].Type == Tile.Variant.Empty &&
				    _maze[i - 1, j].Type == Tile.Variant.Wall &&
				    _maze[i, j + 1].Type == Tile.Variant.Wall &&
				    _maze[i, j - 1].Type == Tile.Variant.Wall) {
					return true;
				}
			}
			catch (IndexOutOfRangeException) { }		
			return false;
		}

		private bool CanSpawnRight(int i, int j) {
			try {
				if (_maze[i, j].Type == Tile.Variant.Empty &&
				    _maze[i + 1, j].Type == Tile.Variant.Wall &&
				    _maze[i, j + 1].Type == Tile.Variant.Wall &&
				    _maze[i, j - 1].Type == Tile.Variant.Wall) {
					return true;
				}
			}
			catch (IndexOutOfRangeException) { }		
			return false;
		}

		private void SpawnChest(Coordinate coordinate, Quaternion rotation) {			
			var chest = Instantiate(_chest, Utils.GetDefaultPositionVector(coordinate, 1.33f), rotation);
			var chestScript = chest.GetComponent<Chest>();
			var itemCount = Random.Range(0, 3);
			
			for (var i = 0; i < itemCount; i++) {
				chestScript.SetChestItems(ItemsCollection.Instance.GetRandomLoot().name, Random.Range(1, 4));
			}
			
			NetworkServer.Spawn(chest);
		}

		public void SpawnChest(Vector3 position, Dictionary<string, int> items) {
			var chest = Instantiate(_chest, position, Quaternion.identity);
			var chestScript = chest.GetComponent<Chest>();
			
			foreach (var item in items) {
				chestScript.SetChestItems(item.Key, item.Value);
			}
			
			NetworkServer.Spawn(chest);
		}

		private void SpawnChests() {			
			var width = _maze.Width;
			var height = _maze.Height;
			
			for (var i = 0; i < width; i++) {
				for (var j = 0; j < height; j++) {
					
					if (Random.Range(0, 101) >= _chanceOfSpawnChest) continue;					
					
					if (CanSpawnBottom(i, j)) {
						SpawnChest(_maze[i,j].Position, Quaternion.Euler(0, 90, 0));						
					}
					
					if (CanSpawnTop(i, j)) {
						SpawnChest(_maze[i, j].Position, Quaternion.Euler(0, 270, 0));						
					}
					
					if (CanSpawnLeft(i, j)) {
						SpawnChest(_maze[i, j].Position, Quaternion.Euler(0, 180, 0));						
					}
					
					if (CanSpawnRight(i, j)) {
						SpawnChest(_maze[i, j].Position, Quaternion.Euler(0, 0, 0));						
					}
					
				}
			}
		}
	}
}
