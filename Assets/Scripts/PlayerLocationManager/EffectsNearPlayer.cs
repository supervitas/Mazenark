﻿using UnityEngine;

namespace PlayerLocationManager {
    public class EffectsNearPlayer : MonoBehaviour {

        private Transform _target;
        private Vector3 _position = new Vector3();


        public float UpdateTime { get; set; }

        public void Start() {
            Y = 0;
            UpdateTime = 2f;
        }

        public float Y { get; set; }


        public void StartEffect(Transform target) {

            _target = target;
            GetComponentInChildren<ParticleSystem>().Play();
            InvokeRepeating("UpdateEffectPosition", 0, UpdateTime);
        }

        public void StopEffect() {
            CancelInvoke("UpdateEffectPosition");
            GetComponentInChildren<ParticleSystem>().Stop();
        }

        private void UpdateEffectPosition() {
            if (_target == null) return;
            _position.Set(_target.position.x, Y, _target.position.z);
            transform.SetPositionAndRotation(_position, Quaternion.identity );
        }
    }
}