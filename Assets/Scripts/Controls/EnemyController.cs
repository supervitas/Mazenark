using UnityEngine;

namespace Controls {
    public class EnemyController : MonoBehaviour {
        [SerializeField] private Animator m_animator;

        // Use this for initialization
        void Start () {
            m_animator.SetBool("Idle", true);
        }

        // Update is called once per frame
        void Update () {

        }
    }
}
