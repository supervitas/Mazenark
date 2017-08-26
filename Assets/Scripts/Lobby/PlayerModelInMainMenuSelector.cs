using MazeBuilder.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelInMainMenuSelector : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;
	public GameObject player3;

	void Start () {
		var playerModels = new CollectionRandom();
		playerModels.Add(player1, typeof(GameObject));
		playerModels.Add(player2, typeof(GameObject));
		playerModels.Add(player3, typeof(GameObject));

		var playerModel = (GameObject) playerModels.GetRandom(typeof(GameObject));

		Instantiate(playerModel, this.transform.position, Quaternion.Euler(0, 180, 0));
	}

}
