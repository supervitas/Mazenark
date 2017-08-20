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
	    
	    [SerializeField]
	    private GameObject _well;


		private BossShieldedControl _bossControls;
	    private GameObject[] _buttons;
	    
	    private int _countOfActiveButtons = 0;


	    public override void OnStartServer() {		
            IsNpc = true;
		    
			_bossControls = GetComponent<BossShieldedControl>();

			_countOfActiveButtons = Random.Range(2, _maxButtons);
			
			_buttons = new GameObject[_countOfActiveButtons];	
			
			for (var i = 0; i < _countOfActiveButtons; i++) {
				SpawnButton(i);
			}

		    var wellPos = transform.position;
		    wellPos.y = 1.63f;
		    NetworkServer.Spawn(Instantiate(_well, wellPos, _well.transform.rotation));

	    }

	    private static float GetRandomFloat(bool needNegative) {
		    return needNegative ? Random.Range(-24, -6) : Random.Range(6, 24);
	    }

		private void SpawnButton(int position) {
			
			var btn = Instantiate(_button);

			_buttons[position] = btn;

			var isNegative = Random.Range(-1, 1) < 0;				
			
			btn.transform.position = new Vector3(gameObject.transform.position.x - GetRandomFloat(isNegative),
				gameObject.transform.position.y, gameObject.transform.position.z - GetRandomFloat(isNegative));
			
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
			
			btnController.UnpressTime = 15f;
			btnController.SetPressCallback(onButtonPressed);
			btnController.SetUnpressCallback(onButtonUnpress);

			NetworkServer.Spawn(btn);
		}

        public override void TakeDamage(int amount, float timeOfDeath = 2f, string whoCasted = "Enemy") {}

	    private void Die() {
		    
		    foreach (var button in _buttons) {
			    button.GetComponent<Collider>().enabled = false;
			    button.GetComponent<GameButton>().RpcDestruct();
			    Destroy(button, 2f);			   
		    }
		    
		    _bossControls.GetComponent<Disolve>().BeginDisolve();
		    _bossControls.Die(2f);
		    
		    Destroy(gameObject, 2f);
		    
		    FindObjectOfType<LootManager>().CreateLoot(gameObject.transform.position, 100f);
	    }
	   
		
	}

}

