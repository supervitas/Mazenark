using UnityEngine;

namespace Walls {

    public class OuterWalls : MonoBehaviour {
        [Tooltip("Maze Walls prefab")]
        public GameObject walls;

        private void Start() {
            for (var i = 0; i < 3; i++) {
                var wall = Instantiate(walls, new Vector3(10, i,
                    20), Quaternion.identity);
//                var renderer = wall.GetComponent<Renderer>();
            }

        }

        private void Update() {

        }
    }
}