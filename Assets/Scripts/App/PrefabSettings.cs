using UnityEngine;

namespace App {
    public class PrefabSettings : MonoBehaviour {
        [Range(0, 100f)] public float SpawnChances = 25f;
    }
}
