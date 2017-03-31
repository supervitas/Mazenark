using UnityEngine;

namespace Prefabs.Particles {
    public class RainFolowingPlayer : MonoBehaviour {

        [SerializeField] private Transform _target;

        private Vector3 _position = new Vector3();
        void Start () {
//            GetComponentInChildren<ParticleSystem>().Stop();
            InvokeRepeating("UpdateRainPosition", 0, 2);
        }

        private void UpdateRainPosition() {
            _position.Set(_target.position.x, 0, _target.position.z);
            transform.SetPositionAndRotation(_position, Quaternion.identity );
        }
    }
}
