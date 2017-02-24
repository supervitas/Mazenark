using System;
using Constants;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Walls {

    public class OuterWalls : MonoBehaviour {
        [Tooltip("Maze Walls prefab")]
        public GameObject [] defaultBiomeWalls;
        public GameObject [] LavaBiomeWalls;
        private readonly MazeSizeGenerator _mazeSize = MazeSizeGenerator.Instance;

		private void generateWall(int size, Quaternion rotationQuaternion,
		    Func<int, Vector3> getPosition, Func<Vector3, int> increment) {
			for (int i = 0; i < size;) {
				var randomWall = defaultBiomeWalls[Random.Range(0, defaultBiomeWalls.Length)];
				var wall = Instantiate(randomWall, getPosition(i), rotationQuaternion);
				var render = wall.GetComponent<Renderer> ();
			    i += increment(render.bounds.size);
			}
		}
        private void Start() {
			generateWall(_mazeSize.X * 8, Quaternion.Euler(0, 90, 0),
			    getPosition: index => new Vector3(-25, 0, index), increment: bounds => (int) bounds.z); // Left

			generateWall(_mazeSize.Y * 8, Quaternion.identity,
			    getPosition: index => new Vector3(index, 0, -25), increment: bounds => (int) bounds.x); // Bottom

            generateWall(_mazeSize.X * 8, Quaternion.Euler(0, 270, 0),
                getPosition: index => new Vector3(_mazeSize.X * 8 +25, 0, index), increment: bounds => (int) bounds.z); // Right

            generateWall(_mazeSize.Y * 8, Quaternion.Euler(0, 180, 0),
                getPosition: index => new Vector3(index, 0, _mazeSize.Y * 8 + 25), increment: bounds => (int) bounds.x); // Top

        }

        private void Update() {

        }

    }
}