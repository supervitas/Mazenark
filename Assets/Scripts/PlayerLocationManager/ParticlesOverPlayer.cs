using UnityEngine;

namespace PlayerLocationManager {
    public class ParticlesOverPlayer : MonoBehaviour {
        public GameObject Particles;
        public Transform Target;

        private GameObject _instancedParticles;

        public void Start() {
            _instancedParticles = Instantiate(Particles, Vector3.back, Quaternion.identity);
            var effects = _instancedParticles.GetComponent<EffectsNearPlayer>();
            effects.Y = 1.5f;
            effects.UpdateTime = 0.3f;
            effects.StartEffect(Target);
        }
    }
}