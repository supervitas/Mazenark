using Controls;
using UnityEngine;

namespace CharacterControllers {
    public class ServerPlayerController : ServerCharacterController {
        
        private PlayerControl _characterControl;
        
        private void Start() {
            IsNpc = false;            
            InvokeRepeating("PlayerUpdate", 0, 0.5f);
            
            _characterControl = GetComponent<PlayerControl>();          
            _characterControl.SetFireballsForServer(5);
            _characterControl.TargetSetFireballs(connectionToClient, 5);
        }
        
        private void OnDestroy() {            
            CancelInvoke("PlayerUpdate");            
        }     
        
        private void OnTriggerEnter(Collider other) { // take loot
            if(!isServer) return;            
            var go = other.gameObject;
            if (!go.CompareTag("Pickable")) return;
            
            _characterControl.SetFireballsForServer(1);
            _characterControl.TargetAddFireballs(connectionToClient, 1);
            
            Destroy(go);
        }
        
        private void PlayerUpdate() {
            if (transform.position.y < -2.5) {
                TakeDamage(100);
            }
        }
    }
}