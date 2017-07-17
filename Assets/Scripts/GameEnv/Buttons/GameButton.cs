using System;
using UnityEngine;
using UnityEngine.Networking;

namespace GameEnv.Buttons {
    public class GameButton : NetworkBehaviour {
        [SerializeField] private Animator _animator;        

        public float UnpressTime = 3f;

        private Action _unpressCallback;
        private Action _pressCallback;

        private bool _isPressed = false;

        public void SetUnpressCallback(Action callback) {
            _unpressCallback = callback;
        }
        
        public void SetPressCallback(Action callback) {
            _pressCallback = callback;
        }

        private void OnTriggerEnter(Collider other) {
            if (!isServer) return;
            
            if (other.CompareTag("Player")) {
                CancelInvoke("Unpress");
                         
                if (_pressCallback != null && _isPressed == false) {
                    _pressCallback();
                }
                
                _isPressed = true;
                _animator.SetBool("Pressed", true);                
            }
        }

        private void OnTriggerExit(Collider other) {
            if (!isServer) return;  
            
            Invoke("Unpress", UnpressTime);
        }

        private void Unpress() {
            _unpressCallback?.Invoke();

            _isPressed = false;
            _animator.SetBool("Pressed", false);
        }

        private void OnDestroy() {
           CancelInvoke("Unpress");
        }
    }
}