using Controls.Bosses;
using Loot;
using UnityEngine;
using UnityEngine.Networking;


namespace CharacterControllers.Enemies.Bosses {
    public class ServerBossMultiplier : ServerCharacterController {
        [SerializeField] [Range(0, 5)] private int _countOfMultiply = 2;
        private int Multiplied { get; set; }

        private void Start() {
            IsNpc = true;
        }

        public override void TakeDamage(int amount, float timeOfDeath = 2f) {
            if (!isServer) return;
            Multiplied++;
            if (Multiplied > _countOfMultiply) {
                var pos = transform.position;
                pos.y = 1.5f;
                FindObjectOfType<LootManager>().CreateLoot(pos, 100f);                              
            } else {
                for (var i = 0; i < Multiplied * 2; i++) {
                    var newBoss = Instantiate(gameObject);
                    newBoss.GetComponent<ServerBossMultiplier>().Multiplied = Multiplied;

                    newBoss.transform.localScale = new Vector3(gameObject.transform.localScale.x / 1.5f,
                        gameObject.transform.localScale.y / 1.5f, gameObject.transform.localScale.z / 1.5f);

                    newBoss.transform.position = new Vector3(gameObject.transform.position.x - Random.Range(-10, 10),
                        gameObject.transform.position.y, gameObject.transform.position.z - Random.Range(-10, 10));

                    NetworkServer.Spawn(newBoss);
                }
            }
            gameObject.GetComponent<BossMultiplierControl>().Die();                
            Destroy(gameObject, timeOfDeath);                   
        }
    }

}

