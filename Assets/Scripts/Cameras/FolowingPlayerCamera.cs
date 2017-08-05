using UnityEngine;

namespace Cameras {
    public class FolowingPlayerCamera : MonoBehaviour {
        [SerializeField] private Transform _target;
        [SerializeField] private float _distance = 3.0f;
        [SerializeField] private float _height = 1.0f;
        [SerializeField] private float _damping = 5.0f;
        [SerializeField] private bool _smoothRotation = true;
        [SerializeField] private float _rotationDamping = 10.0f;
        [SerializeField] private float _distanceBetweenRays = 0.2f;

        [SerializeField] private Vector3 _targetLookAtOffset;
        [SerializeField] private float _rayMaximum = 2f;
        public LayerMask collisionLayerMask;

        private RaycastHit m_hitWall;        

        private void Start() {
            m_hitWall = new RaycastHit();                        
        }

        public void SetPlayer(Transform player) {
            _target = player;
        }
        

        private void FixedUpdate() {
            if (!_target) return;

            Vector3 wantedPosition = _target.TransformPoint(0, _height, -_distance);            
                                                                           
            if (Physics.Raycast(_target.transform.position, -_target.transform.forward, out m_hitWall, _rayMaximum,
                    collisionLayerMask)) {                   
                wantedPosition.x = m_hitWall.point.x;
                wantedPosition.z = m_hitWall.point.z;
                wantedPosition.y = Mathf.Lerp(m_hitWall.point.y + _height, wantedPosition.y,
                    Time.deltaTime * _damping);                                                                                     
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
