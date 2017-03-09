using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MazeBuilder.Walls {

    public class OuterWalls : MonoBehaviour {
        [Tooltip("Maze Walls prefab")]
        public GameObject [] DefaultBiomeWalls;
        public GameObject [] LavaBiomeWalls;
        private readonly MazeSizeGenerator _mazeSize = MazeSizeGenerator.Instance;

		private void GenerateWall(int size, Quaternion rotationQuaternion,
		    Func<float, Vector3> getPosition, Func<Vector3, float> increment) {
		    var rootObjForWalls = new GameObject {name = "WallsGroup", isStatic = true};
		    for (float i = 0; i < size;) {
				var randomWall = DefaultBiomeWalls[Random.Range(0, DefaultBiomeWalls.Length)];
				var wall = Instantiate(randomWall, getPosition(i), rotationQuaternion);
				var render = wall.GetComponent<Renderer>();
			    wall.transform.parent = rootObjForWalls.transform;
			    i += increment(render.bounds.size) - Random.Range(3, 20); // to have no gaps and create uniq wall
			}
		    StaticBatchingUtility.Combine(rootObjForWalls.gameObject);
		}

        private void Start() {
			GenerateWall(_mazeSize.X * Constants.Maze.TILE_SIZE + 25, Quaternion.Euler(0, 90, 0),
			    getPosition: index => new Vector3(-25, 0, index), increment: bounds => bounds.z); // Left

			GenerateWall(_mazeSize.Y * Constants.Maze.TILE_SIZE + 25, Quaternion.identity,
			    getPosition: index => new Vector3(index, 0, -25), increment: bounds => bounds.x); // Bottom

            GenerateWall(_mazeSize.X * Constants.Maze.TILE_SIZE + 25, Quaternion.Euler(0, 270, 0),
                getPosition: index => new Vector3(_mazeSize.X * 8 + 25, 0, index), increment: bounds => bounds.z); // Right

            GenerateWall(_mazeSize.Y * Constants.Maze.TILE_SIZE + 25, Quaternion.Euler(0, 180, 0),
                getPosition: index => new Vector3(index, 0, _mazeSize.Y * 8 + 25), increment: bounds => bounds.x); // Top

        }

        private void Update() {

        }

    }
}