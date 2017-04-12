using UnityEngine;
using UnityEngine.Networking;

namespace Controls {
    public class EnemyController : NetworkBehaviour {
        [SerializeField] private Animator m_animator;

        // Use this for initialization
        void Start () {
            Debug.Log(transform.position.x);
            m_animator.SetBool("Idle", true);
        }

        // Update is called once per frame
        void Update () {

        }
    }
}
