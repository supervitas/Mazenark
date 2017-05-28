namespace CharacterControllers.Enemies {
    public class ServerEnemyController : ServerCharacterController {
        
        private void Start() {
            IsNpc = true;
            DestroyOnDeath = true;            
        }
    }
}