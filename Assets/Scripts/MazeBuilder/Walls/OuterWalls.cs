﻿using System;
using UnityEngine;
using MazeBuilder.Constants;
using Random = UnityEngine.Random;

namespace MazeBuilder.Walls {

    public class OuterWalls : MonoBehaviour {
        [Tooltip("Maze Walls prefab")]
        public GameObject [] DefaultBiomeWalls;
        public GameObject [] LavaBiomeWalls;
        private readonly MazeSizeGenerator _mazeSize = MazeSizeGenerator.Instance;

		private void generateWall(int size, Quaternion rotationQuaternion,
		    Func<float, Vector3> getPosition, Func<Vector3, float> increment) {
			for (float i = 0; i < size;) {
				var randomWall = DefaultBiomeWalls[Random.Range(0, DefaultBiomeWalls.Length)];
				var wall = Instantiate(randomWall, getPosition(i), rotationQuaternion);
				var render = wall.GetComponent<Renderer> ();
			    i += increment(render.bounds.size);
			}
		}
        private void Start() {
			generateWall(_mazeSize.X * Constants.Maze.TILE_SIZE + 25, Quaternion.Euler(0, 90, 0),
			    getPosition: index => new Vector3(-25, 0, index), increment: bounds => bounds.z); // Left

			generateWall(_mazeSize.Y * Constants.Maze.TILE_SIZE + 25, Quaternion.identity,
			    getPosition: index => new Vector3(index, 0, -25), increment: bounds => bounds.x); // Bottom

            generateWall(_mazeSize.X * Constants.Maze.TILE_SIZE + 25, Quaternion.Euler(0, 270, 0),
                getPosition: index => new Vector3(_mazeSize.X * 8 + 25, 0, index), increment: bounds => bounds.z); // Right

            generateWall(_mazeSize.Y * Constants.Maze.TILE_SIZE + 25, Quaternion.Euler(0, 180, 0),
                getPosition: index => new Vector3(index, 0, _mazeSize.Y * 8 + 25), increment: bounds => bounds.x); // Top

        }

        private void Update() {

        }

    }
}