using System.Collections.Generic;
using System.Linq;
using MazeBuilder;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using MazeBuilder.Utility;

namespace Controls {
    public class EnemyController : NetworkBehaviour {
        [SerializeField] private Animator m_animator;
        private NavMeshAgent _agent;
        private readonly List <Vector3> points = new List<Vector3>();
        private int _destPoint = 1;

        void Start () {
            m_animator.SetBool("Idle", true);
            GeneratePatroolPoints();

            _agent = GetComponent<NavMeshAgent>();
            _agent.autoBraking = false;

            GotoNextPoint();
        }

        void GeneratePatroolPoints() {
            points.Add(transform.position); // first point is where enemy was spawned

            var coord = Utils.TransformWorldToLocalCoordinate(transform.position.x, transform.position.y);
            var currBiome = App.AppManager.Instance.MazeInstance.Maze[coord.X, coord.Y].Biome;

            var tileList = (from biome in App.AppManager.Instance.MazeInstance.Maze.Biomes
                where biome.biome == currBiome
                from tile in biome.tiles
                where tile.Type == Tile.Variant.Empty
                select tile
            ).Take(3);

            foreach (var tile in tileList) {
                points.Add(Utils.TransformToWorldCoordinate(tile.Position));
            }

        }

        void GotoNextPoint() {
            // Set the agent to go to the currently selected destination.
            _agent.destination = points[_destPoint];

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            _destPoint = (_destPoint + 1) % points.Count;
        }

        // Update is called once per frame
        void Update () {
//            if (_agent.remainingDistance < 0.5f)
//                GotoNextPoint();
        }
    }
}
