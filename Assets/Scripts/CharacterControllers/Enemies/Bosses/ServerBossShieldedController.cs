using System;
using Controls.Bosses;
using GameEnv.Buttons;
using GameEnv.GameEffects;
using Loot;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;


namespace CharacterControllers.Enemies.Bosses {
    public class ServerBossShieldedController : ServerCharacterController {
	
	    [SerializeField]
		private int _maxButtons = 4;						

		[SerializeField]
		private GameObject _button;


		private BossShieldedControl _bossControls;
	    private GameObject[] _buttons;
	    
	    private int _countOfActiveButtons = 0;
		
		
		private void Start() {
			if (!isServer) return;
			
            IsNpc = true;			

			_bossControls = GetComponent<BossShieldedControl>();

			_countOfActiveButtons = Random.Range(1, _maxButtons);
			
			_buttons = new GameObject[_countOfActiveButtons];	
			
			for (var i = 0; i < _countOfActiveButtons; i++) {
				SpawnButton(i);
			}

		}

		private void SpawnButton(int position) {
			
			var btn = Instantiate(_button);

			_buttons[position] = btn;
			
			btn.transform.position = new Vector3(gameObject.transform.position.x - Random.Range(-12, 12),
				gameObject.transform.position.y, gameObject.transform.position.z - Random.Range(-12, 12));
			
			Action onButtonPressed = () => {
				_countOfActiveButtons--;

				if (_countOfActiveButtons == 0) {
					Die();
				}
			};
			
			Action onButtonUnpress= () => {
				_countOfActiveButtons++;		
			};
			
			var btnController = btn.GetComponent<GameButton>();
			
			btnController.UnpressTime = 7f;
			btnController.SetPressCallback(onButtonPressed);
			btnController.SetUnpressCallback(onButtonUnpress);

			NetworkServer.Spawn(btn);
		}

        public override void TakeDamage(int amount, float timeOfDeath = 2f) {}

	    private void Die() {
		    
		    foreach (var button in _buttons) {
			    button.GetComponent<Disolve>().BeginDisolve();
			    button.GetComponent<Collider>().enabled = false;
			    Destroy(button, 2f);			   
		    }
		    
		    _bossControls.GetComponent<Disolve>().BeginDisolve();
		    _bossControls.Die(2f);
		    
		    Destroy(gameObject, 2f);
		    
		    FindObjectOfType<LootManager>().CreateLoot(gameObject.transform.position, 100f);
	    }
		
	}

}

