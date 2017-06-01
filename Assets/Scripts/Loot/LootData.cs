using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loot {
	public class LootData : MonoBehaviour {
		public string lootName;

		[Range(0.0f, 360.0f)]
		[Tooltip("Rotational speed. Measured in degrees per second.")]
		public float rotationSpeed = 360.0f;

		[Tooltip("Unchecked is CW, checked is CCW. If viewed from top.")]
		public bool isReversed = false;

		[Range(0.0f, 2.0f)]
		[Tooltip("Amplitude of oscillations. Measured in meters.")]
		public float amplitude = 1.0f;

		//[Range(0.0f, 2.0f)]
		//[Tooltip("How fast up-down oscillations are made relatively to rotation.")]
		//public float oscillationSpeed = 0.5f;

		[Tooltip("Offset for preventing spawning in floor.")]
		public Vector3 offset = new Vector3();

		// Use this for initialization
		void Start() {
			this.gameObject.transform.Translate(offset);
		}

		// Update is called once per frame
		void Update() {
			// rotation is previous rotation plus delta rotation over time.
			float deltaRotationDeg = (isReversed ? -1.0f : 1.0f) * rotationSpeed * Time.deltaTime;
			float rotationDeg = gameObject.transform.rotation.eulerAngles.y + deltaRotationDeg;

			float phase = Mathf.Sin(rotationDeg * Mathf.Deg2Rad);
			Vector3 offset = new Vector3(0, 1.0f, 0) * amplitude * phase;

			gameObject.transform.Rotate(0, deltaRotationDeg, 0);
			gameObject.transform.Translate(offset);
		}

	}
}