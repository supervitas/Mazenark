using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFolowingPlayer : MonoBehaviour {

    [SerializeField] private Transform _target;

    private Vector3 _position = new Vector3();
	// Use this for initialization
	void Start () {
	    InvokeRepeating("UpdateRainPosition", 0, 2);
	}
	
	// Update is called once per frame
    private void UpdateRainPosition() {
        _position.Set(_target.position.x, 0, _target.position.z);
        transform.SetPositionAndRotation(_position, Quaternion.identity );
    }
}
