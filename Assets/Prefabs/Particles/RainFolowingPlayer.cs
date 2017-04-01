using UnityEngine;

namespace Prefabs.Particles {
    public class RainFolowingPlayer : MonoBehaviour {

        private Transform _target;
        private Vector3 _position = new Vector3();

        public void StartRain(Transform target) {
            _target = target;
            GetComponentInChildren<ParticleSystem>().Play();
            InvokeRepeating("UpdateRainPosition", 0, 2);
        }

        public void StopRain() {
            CancelInvoke("UpdateRainPosition");
            GetComponentInChildren<ParticleSystem>().Stop();
        }

        private void UpdateRainPosition() {
            _position.Set(_target.position.x, 0, _target.position.z);
            transform.SetPositionAndRotation(_position, Quaternion.identity );
        }
    }
}
