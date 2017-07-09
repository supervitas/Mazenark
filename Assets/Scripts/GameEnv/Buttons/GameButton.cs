using UnityEngine;
using UnityEngine.Networking;

namespace GameEnv.Buttons {
    public class GameButton : MonoBehaviour {
        [SerializeField] private Animator _animator;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                _animator.SetBool("Pressed", true);
                Invoke("Unpress", 3f);
            }
        }

        private void Unpress(){
            _animator.SetBool("Pressed", false);
        }
    }
}