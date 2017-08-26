using System;
using App;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MazeBuilder.Walls {

    public class OuterWalls : MonoBehaviour {
        [Tooltip("Maze Walls prefab")]
        public GameObject [] DefaultBiomeWalls;
        public GameObject [] LavaBiomeWalls;

		private void GenerateWall(int size, Quaternion rotationQuaternion,
		    Func<float, Vector3> getPosition, Func<Vector3, float> increment) {
		    var rootObjForWalls = new GameObject {name = "WallsGroup"};
		    for (float i = 0; i < size;) {
				var randomWall = DefaultBiomeWalls[Random.Range(0, DefaultBiomeWalls.Length)];
				var wall = Instantiate(randomWall, getPosition(i), rotationQuaternion);
				var render = wall.GetComponent<Renderer>();
			    wall.transform.parent = rootObjForWalls.transform;
			    i += increment(render.bounds.size) - Random.Range(3, 20); // to have no gaps and create uniq wall
			}
		}

        private void Start() {
            var mazeSize = AppManager.Instance.MazeInstance;

            GenerateWall(mazeSize.Width * Constants.Maze.TILE_SIZE + 25, Quaternion.Euler(0, 90, 0),
                getPosition: index => new Vector3(-25, -8, index), increment: bounds => bounds.z); // Left

            GenerateWall(mazeSize.Height * Constants.Maze.TILE_SIZE + 25, Quaternion.identity,
                getPosition: index => new Vector3(index, -8, -25), increment: bounds => bounds.x); // Bottom

            GenerateWall(mazeSize.Width * Constants.Maze.TILE_SIZE + 25, Quaternion.Euler(0, 270, 0),
                getPosition: index => new Vector3(mazeSize.Width * 8 + 25, -8, index), increment: bounds => bounds.z); // Right

            GenerateWall(mazeSize.Height * Constants.Maze.TILE_SIZE + 25, Quaternion.Euler(0, 180, 0),
                getPosition: index => new Vector3(index, -8, mazeSize.Height * 8 + 25), increment: bounds => bounds.x); // Top

        }

    }
}