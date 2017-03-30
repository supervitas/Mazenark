using App.EventSystem;
using UnityEngine;

namespace App {
    public class DynamicOC : MonoBehaviour {
        private GameObject[] _allObjects;
        [SerializeField] private Transform _target;
        // Use this for initialization
        void Start () {
            AppManager.Instance.EventHub.Subscribe("mazedrawer:placement_finished", InitOccluder, this);

        }

        private void InitOccluder(object sender, EventArguments eventArguments) {
            _allObjects = GameObject.FindGameObjectsWithTag("Occlusion");
//            InvokeRepeating("DisableAll", 0, 10);
        }

        // Update is called once per frame
        void DisableAll () {
            foreach (GameObject a in _allObjects) {
//                foreach (var child in a.g) {
//
//                }
                a.GetComponent<Renderer>().enabled = false;
            }
        }

        private void FixedUpdate() {
//            RaycastHit hit;
//            Vector3 front = _target.transform.TransformDirection( Vector3.forward);
//
//            if (Physics.Raycast(_target.position, front, out hit, 200f)) {
//                hit.transform.GetComponent<Renderer>().enabled = true;
//            }
        }
    }
}
