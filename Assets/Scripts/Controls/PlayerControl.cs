using System.Collections.Generic;
using System.Linq;
using App;
using App.Eventhub;
using Cameras;
using CharacterControllers;
using Items;
using Loot;
using Ui;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Controls {
    [NetworkSettings(channel = 1, sendInterval = 0.2f)]
    public class PlayerControl : NetworkBehaviour {

        private enum ControlMode {
            Tank,
            Direct
        }
        
        [SyncVar(hook = nameof(OnSetName))] 
        private string _playerName;
        

        private readonly Dictionary<string, int> _playerItems = new Dictionary<string, int>();        

        [SerializeField] private float m_moveSpeed = 2;
        [SerializeField] private float m_turnSpeed = 200;
        [SerializeField] private float m_jumpForce = 4;
        [SerializeField] private Animator m_animator;
        [SerializeField] private Rigidbody m_rigidBody;
        [SerializeField] private GameObject spellCastEffect;

        [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

        public GameObject PlayerCamera;
        private Camera _camera;       

        private GameObject _activeItem;

        private ServerPlayerController _serverPlayerController;
        
        private Text _spellText;
        private float _castTime;
        private float _timeCasted;
        private float _timeCooled;
        private float _coolDown = 0.2f;
        
        private readonly Vector3 _zeroVector = Vector3.zero;
        
        
        private SpellCast _uiSpellCast;
        private GameObject _spellEffect;

        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;
        private readonly float m_walkScale = 0.33f;
        private readonly float m_backwardsWalkScale = 0.16f;
        private readonly float m_backwardRunScale = 0.66f;

        private bool m_wasGrounded;
        private Vector3 m_currentDirection = Vector3.zero;

        private float m_jumpTimeStamp = 0;
        private float m_minJumpInterval = 0.25f;

        private bool m_isGrounded;
        private readonly List<Collider> m_collisions = new List<Collider>();              

        private GameGui _gameGui;       

        private void OnCollisionEnter(Collision collision) {
            ContactPoint[] contactPoints = collision.contacts;
            foreach (ContactPoint t in contactPoints) {
                if (Vector3.Dot(t.normal, Vector3.up) > 0.5f) {
                    if (!m_collisions.Contains(collision.collider)) {
                        m_collisions.Add(collision.collider);
                    }
                    m_isGrounded = true;
                }
            }
        }

        private void OnDestroy() {            
            if (!isLocalPlayer) return;
            
            AppManager.Instance.TurnOnMainCamera();
            AppManager.Instance.EventHub.UnsubscribeFromAll(this);
            _uiSpellCast.Reset();
        }

        private void OnCollisionStay(Collision collision) {
            ContactPoint[] contactPoints = collision.contacts;
            bool validSurfaceNormal = contactPoints.Any(t => Vector3.Dot(t.normal, Vector3.up) > 0.5f);

            if (validSurfaceNormal) {
                m_isGrounded = true;
                if (!m_collisions.Contains(collision.collider)) {
                    m_collisions.Add(collision.collider);
                }
            }
            else {
                if (m_collisions.Contains(collision.collider)) {
                    m_collisions.Remove(collision.collider);
                }
                if (m_collisions.Count == 0) {
                    m_isGrounded = false;
                }
            }
        }
        
        public override void OnStartClient() {
            var textMesh = GetComponentInChildren<TextMesh>();
            textMesh.text = _playerName;            
        }

        public override void OnStartLocalPlayer() {
            if (!isLocalPlayer) return;
            
            AppManager.Instance.EventHub.Subscribe("ItemChanged", OnActiveItemChanged, this);
            
            AppManager.Instance.TurnOffAndSetupMainCamera(); 
            
            _gameGui = FindObjectOfType<GameGui>();
            _spellText = GameObject.FindGameObjectWithTag("UISpellName").GetComponent<Text>();

            var cam = Instantiate(PlayerCamera);
            cam.GetComponent<FolowingPlayerCamera>().SetPlayer(transform);
            _camera = cam.GetComponent<Camera>();

            _uiSpellCast = FindObjectOfType<SpellCast>();
            
            _serverPlayerController = GetComponent<ServerPlayerController>();
                       
            _serverPlayerController.CmdPlayerReady();
            
            var textMesh = GetComponentInChildren<TextMesh>();
            textMesh.gameObject.SetActive(false);

            _spellEffect = Instantiate(spellCastEffect, gameObject.transform);
            var pos = _spellEffect.transform.position;
            pos.y += 1f;
            _spellEffect.transform.position = pos; 
            _spellEffect.SetActive(false);
        }

        private void OnActiveItemChanged(object sender, EventArguments e) {
            _activeItem = ItemsCollection.Instance.GetItemByName(e.Message);
            _castTime = _activeItem.GetComponent<Weapon>().GetCastTime();
            _spellText.text = e.Message;
            _serverPlayerController.CmdSetActiveItem(e.Message);
        }


        private void OnCollisionExit(Collision collision) {
            if (m_collisions.Contains(collision.collider)) {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) {
                m_isGrounded = false;
            }
        }

        private bool CheckAndFire() {
            if (!Input.GetMouseButton(0) || _activeItem == null || _playerItems[_activeItem.name] <= 0) return false;
            
            m_animator.SetFloat("MoveSpeed", 0);
            _timeCasted += Time.deltaTime;
            _spellEffect.SetActive(true);

            if (_timeCasted > 0.2) {
                _uiSpellCast.SetProgress(_timeCasted / _castTime * 100);
            }

            if (_timeCasted >= _castTime) {
                _timeCasted = 0;
                _uiSpellCast.Reset();
                                 
                var direction = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 25);
                direction = _camera.ScreenToWorldPoint(direction);
                
                _serverPlayerController.CmdFire(direction);
                _playerItems[_activeItem.name]--;
                _gameGui.ModifyItemCount(_activeItem.name, _playerItems[_activeItem.name].ToString());

                _timeCooled = 0;

                if (_playerItems[_activeItem.name] <= 0) {
                    _gameGui.DisableItem(_activeItem.name);
                    _playerItems.Remove(_activeItem.name);
                    _activeItem = null;
                }
            }
            return true;
        }

        private bool CheckCooldown() {
            if (_timeCooled <= _coolDown) {
                _timeCooled += Time.deltaTime;
                return false;
            }
            return true;
        }

        private void Update() {
            if (!isLocalPlayer) return;

//            m_animator.SetBool("Grounded", m_isGrounded);

            if (CheckCooldown() && CheckAndFire()) return;                            
            
            _timeCasted = 0;
            _uiSpellCast.Reset();
            _spellEffect.SetActive(false);

            switch (m_controlMode) {
                case ControlMode.Direct:
                    DirectUpdate();
                    break;

                case ControlMode.Tank:
                    TankUpdate();
                    break;

                default:
                    Debug.LogError("Unsupported state");
                    break;
            }

            m_wasGrounded = m_isGrounded;
        }

        private void TankUpdate() {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            

            if (v < 0) {                    
                v *= m_backwardRunScale;                
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
            transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

            m_animator.SetFloat("MoveSpeed", m_currentV);

            JumpingAndLanding();
        }

        private void DirectUpdate() {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            Transform camera = _camera.transform;
  

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != _zeroVector) {
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                transform.rotation = Quaternion.LookRotation(m_currentDirection);
                transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }

            JumpingAndLanding();
        }

        private void JumpingAndLanding() {
            bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

            if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space)) {
                m_jumpTimeStamp = Time.time;
                m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            }

            if (!m_wasGrounded && m_isGrounded) {
                m_animator.SetTrigger("Land");
            }

            if (!m_isGrounded && m_wasGrounded) {
                m_animator.SetTrigger("Jump");
            }
        }       
        
                
        private void OnSetName(string playerNickName) {
            if (isLocalPlayer) return;
            
            var textMesh = GetComponentInChildren<TextMesh>();
            textMesh.text = playerNickName;            
        }
        
        
        public void SetPlayerName(string playerName) {            
            _playerName = playerName;            
        }
        
        [TargetRpc]
        public void TargetSetPlayerItems(NetworkConnection target, string itemName, int count) {
            if (!_playerItems.ContainsKey(itemName)) {                
                _playerItems.Add(itemName, 0);
                _gameGui.AddItem(itemName, count.ToString());
            }            
            _playerItems[itemName] += count;
            _gameGui.ModifyItemCount(itemName, _playerItems[itemName].ToString());           
        }       
    }
}