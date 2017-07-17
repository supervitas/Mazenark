using UnityEngine;

namespace PlayerLocationManager {
    public class ParticlesOverPlayer : MonoBehaviour {
        public GameObject Particles;
        public GameObject GroundFog;
        public Transform Target;

        private GameObject _instancedParticles;
        private GameObject _instancedGroundFog;

        public void Start() {
            _instancedParticles = Instantiate(Particles, Vector3.back, Quaternion.identity);
            var effects = _instancedParticles.GetComponent<EffectsNearPlayer>();
            effects.Y = 3.5f;
            effects.UpdateTime = 0.3f;
            effects.StartEffect(Target);
            
            _instancedGroundFog = Instantiate(GroundFog, Vector3.back, Quaternion.Euler(90, 0, 0));
            var fogEffects = _instancedGroundFog.GetComponent<EffectsNearPlayer>();
            fogEffects.Y = -12f;
            fogEffects.UpdateTime = 0.3f;
            fogEffects.StartEffect(Target);
        }
    }
}