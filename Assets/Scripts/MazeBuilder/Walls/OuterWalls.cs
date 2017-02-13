using Constants;
using UnityEngine;

namespace Walls {

    public class OuterWalls : MonoBehaviour {
        [Tooltip("Maze Walls prefab")]
        public GameObject walls;

        private void Start() {
            var mazeSize = MazeSizeGenerator.Instance;
            for (var i = 0; i < 1; i++) {
                var wall = Instantiate(walls, new Vector3(-10, 32,
                    32), Quaternion.Euler(0, 0, 0));
                wall.transform.localScale = new Vector3(10, mazeSize.Y, 64);
                var renderer = wall.GetComponent<Renderer>();

            }

        }

        private void Update() {

        }
    }
}