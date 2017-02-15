using System.Runtime.InteropServices;
using Constants;
using UnityEngine;

namespace Walls {

    public class OuterWalls : MonoBehaviour {
        [Tooltip("Maze Walls prefab")]
        public GameObject [] defaultBiomeWalls;
        public GameObject [] LavaBiomeWalls;
        private readonly MazeSizeGenerator _mazeSize = MazeSizeGenerator.Instance;

        private void generateWall(float x, int y, float z, int rotation) {
            var wall = Instantiate(defaultBiomeWalls[Random.Range(0, defaultBiomeWalls.Length)], new Vector3(x, y,
                z), Quaternion.Euler(0, rotation, 0));
//            wall.transform.localScale = new Vector3(20, _mazeSize.Y, _mazeSize.Y * 10);
            wall.GetComponent<Renderer>();
        }

        private void Start() {
            for (var i = 0; i < _mazeSize.X ; i++) {
                var wall = Instantiate(defaultBiomeWalls[Random.Range(0, defaultBiomeWalls.Length)], new Vector3(-20, 0,
                    i * 8), Quaternion.Euler(Random.Range(0, 45), 90, Random.Range(0, 45)));
                wall.transform.localScale = new Vector3(8, 8, 8);
                wall.GetComponent<Renderer>();
            }

//            generateWall(x: -10, y: 32, z: (float) (_mazeSize.Y * 4.5), rotation: 0);
//            generateWall(x: (float) (_mazeSize.X * 5), y: _mazeSize.Y / 2, z: -(_mazeSize.Y / 2), rotation: 90);
//            generateWall(x: _mazeSize.X * 10, y: 32, z: (float) (_mazeSize.Y * 4.5), rotation: 0);
//            generateWall(x: (float) (_mazeSize.X * 5), y: _mazeSize.Y / 2, z: (_mazeSize.Y * 10) - 20 , rotation: 90);
        }

        private void Update() {

        }
    }
}