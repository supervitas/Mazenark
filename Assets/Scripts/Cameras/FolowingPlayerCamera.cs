﻿using UnityEngine;
using UnityEngine.AI;

namespace Cameras {
    public class FolowingPlayerCamera : MonoBehaviour {
        [SerializeField] private Transform _target;
        [SerializeField] private float _distance = 3.0f;
        [SerializeField] private float _height = 1.0f;
        [SerializeField] private float _damping = 5.0f;
        [SerializeField] private bool _smoothRotation = true;
        [SerializeField] private float _rotationDamping = 10.0f;

        [SerializeField] private Vector3 _targetLookAtOffset; // allows offsetting of camera lookAt, very useful for low bumper heights

        [SerializeField] private float _bumperDistanceCheck = 2.5f; // length of bumper ray
        [SerializeField] private float _bumperCameraHeight = 1.0f; // adjust camera height while bumping


        public void SetPlayerTransforms(Transform player) {
            _target = player;
            GetComponent<Camera>().transform.parent = _target;
        }


        private void FixedUpdate() {
            if (!_target) return;

            Vector3 wantedPosition = _target.TransformPoint(0, _height, -_distance);

            // check to see if there is anything behind the target
            RaycastHit hit;
            Vector3 back = _target.transform.TransformDirection(-1 * Vector3.forward);
            // cast the bumper Sphere out from rear and check to see if there is anything behind
            if (Physics.SphereCast(_target.transform.position, _target.transform.position.y, back, out hit, _bumperDistanceCheck)
                && !hit.transform.CompareTag("Player") && hit.transform.GetComponent<MeshRenderer>().bounds.size.y > 2) {
                wantedPosition.x = hit.point.x;
                wantedPosition.z = hit.point.z;
                wantedPosition.y = Mathf.Lerp(hit.point.y + _bumperCameraHeight, wantedPosition.y, Time.deltaTime * _damping);
            }
            transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * _damping);

            Vector3 lookPosition = _target.TransformPoint(_targetLookAtOffset);

            if (_smoothRotation) {
                Quaternion wantedRotation = Quaternion.LookRotation(lookPosition - transform.position, _target.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation,
                    Time.deltaTime * _rotationDamping);
            } else {
                transform.rotation = Quaternion.LookRotation(lookPosition - transform.position, _target.up);
            }
        }

    }
}