using System.Runtime.InteropServices;
using Constants;
using UnityEngine;

namespace Walls {

    public class OuterWalls : MonoBehaviour {
        [Tooltip("Maze Walls prefab")]
        public GameObject walls;
        private readonly MazeSizeGenerator _mazeSize = MazeSizeGenerator.Instance;

        private void generateWall(float x, int y, float z, int rotation) {
            var wall = Instantiate(walls, new Vector3(x, y,
                z), Quaternion.Euler(0, rotation, 0));
            wall.transform.localScale = new Vector3(20, _mazeSize.Y, _mazeSize.Y * 10);
            wall.GetComponent<Renderer>();
        }

        private void Start() {
            generateWall(x: -10, y: 32, z: (float) (_mazeSize.Y * 4.5), rotation: 0);
            generateWall(x: (float) (_mazeSize.X * 5), y: _mazeSize.Y / 2, z: -(_mazeSize.Y / 2), rotation: 90);
            generateWall(x: _mazeSize.X * 10, y: 32, z: (float) (_mazeSize.Y * 4.5), rotation: 0);
            generateWall(x: (float) (_mazeSize.X * 5), y: _mazeSize.Y / 2, z: (_mazeSize.Y * 10) - 20 , rotation: 90);



        }

        private void Update() {

        }
    }
}