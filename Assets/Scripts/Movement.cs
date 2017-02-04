using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update() {
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 15000.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 15000.0f;

        GetComponent<Rigidbody>().AddTorque(new Vector3(0, x, 0));
//        GetComponent<Rigidbody>().AddForce(new Vector3(x, 0, z));
	}
}
