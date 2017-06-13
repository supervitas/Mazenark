using UnityEngine;

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
        
        private bool _wasColided = false;

        public void SetPlayerTransforms(Transform player) {
            _target = player;
            GetComponent<Camera>().transform.parent = _target;
        }

        private void OnTriggerStay(Collider other) {
            if (!_target) return;
            
            _wasColided = true;
            
            var wantedPosition = _target.TransformPoint(0, _height, -_distance);
            
            wantedPosition.x = other.transform.position.x;
            wantedPosition.z = other.transform.position.z;
            wantedPosition.y = Mathf.Lerp(other.transform.position.y + _bumperCameraHeight, wantedPosition.y, Time.deltaTime * _damping); 
            
            MoveCamera(wantedPosition);
            
            _wasColided = false;
        }

        private void FixedUpdate() {
            if (!_target && _wasColided) return;           
            
            Vector3 wantedPosition = _target.TransformPoint(0, _height, -_distance);
            
            MoveCamera(wantedPosition);                                       
        }

        private void MoveCamera(Vector3 wantedPosition) {
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