using Controls.Bosses;
using Loot;
using UnityEngine;
using UnityEngine.Networking;


namespace CharacterControllers.Enemies.Bosses {
    public class ServerBossShieldedController : ServerCharacterController {
	
	    [SerializeField]
		private int _maxShieldedButtons = 5;						

		[SerializeField]
		private GameObject _shieldPrefab = null;


		private BossShieldedControl _bossControls = null;
		
		
		private void Start() {
			if (!isServer) return;
            IsNpc = true;			

			_bossControls = GetComponent<BossShieldedControl>();			

			for (int i = 0; i < Random.Range(2, _maxShieldedButtons); i++) {
				SpawnShield(i);
			}

		}

		private void SpawnShield(int i) {
//			_shields[i] = Instantiate(_shieldPrefab);			
//
//			_shieldsControls[i] = _shields[i].GetComponent<BossShieldShieldedControl>();
//			_shieldsControls[i].Controller = this;
//			_shieldsControllers[i] = _shields[i].GetComponent<ServerBossShieldShieldedController>();
//			_shieldsControllers[i].Parent = this;
//
//			var room = _bossControls.GetSpawnRoom();			
//			_shields[i].transform.position = new Vector3(gameObject.transform.position.x - Random.Range(-12, 12),
//				gameObject.transform.position.y, gameObject.transform.position.z - Random.Range(-12, 12));
//
//			NetworkServer.Spawn(_shields[i]);
			
		}

        public override void TakeDamage(int amount, float timeOfDeath = 2f) {
            if (!isServer) return;
			TakeDamage(amount, gameObject);
		}

		public void TakeDamage(int amount, GameObject target, float timeOfDeath = 2f) {
			if (!isServer) return;

//			if (target == gameObject && _currentNumberOfShields == 0) {
//				_bossControls.Die();
//				// LootManager
//				// Statistics
//				Destroy(this, timeOfDeath);
//			}
//
//			for (int i = 0; i < _desiredNumberOfShields; i++) {
//				if (target == _shields[i]) {
//					_currentNumberOfShields--;
//					_shieldsControls[i].Die();
//					// LootManager
//					// Statistics
//					Destroy(_shields[i], timeOfDeath);
//					_shields[i] = null;
//
//				}
//			}
		}
	}



}

