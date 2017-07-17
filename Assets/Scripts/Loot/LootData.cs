using UnityEngine;

namespace Loot {
	public class LootData : MonoBehaviour {
		public string lootName;

		[Range(0.0f, 360.0f)]
		[Tooltip("Rotational speed. Measured in degrees per second.")]
		[SerializeField] private float rotationSpeed = 360.0f;

		[Tooltip("Unchecked is CW, checked is CCW. If viewed from top.")]
		[SerializeField] private bool isReversed;

		[Range(0.0f, 2.0f)]
		[Tooltip("Amplitude of oscillations. Measured in meters.")]
		[SerializeField] private float amplitude = 1.0f;

		//[Range(0.0f, 2.0f)]
		//[Tooltip("How fast up-down oscillations are made relatively to rotation.")]
		//public float oscillationSpeed = 0.5f;

		[Tooltip("Offset for preventing spawning in floor.")]
		[SerializeField] private Vector3 offset;

		// Use this for initialization
		private void Start() {
			gameObject.transform.Translate(offset);
		}

		// Update is called once per frame
		private void Update() {
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