using App.EventSystem;
using MazeBuilder.Utility;
using UnityEngine;

namespace MazeBuilder.BiomeGenerators {
    public class FireGenerator : AbstractBiomeGenerator {
        #region BiomeWalls
        [Header("Biome Walls")]
        public GameObject FlatWall;
        #endregion

        #region BiomeFloor
        [Header("Biome Floor")]
        public GameObject Floor;
        #endregion

        private void Awake() {
            base.Awake();
            Eventhub.Subscribe("lol", HandleCustomEvent);
        }
        void HandleCustomEvent(object sender, EventArguments e) {
            Debug.Log( " received this message: "+ e.Message);
//            Eventhub.Unsubscribe("lol", HandleCustomEvent);
        }


        public override GameObject CreateWall(Biome biome, Coordinate coordinate, Maze maze) {
            var go = Instantiate(FlatWall, GetDefaultPositionVector(coordinate, true), Quaternion.identity);
            return go;
        }
        public override GameObject CreateFloor(Biome biome, Coordinate coordinate, Maze maze) {
            var go = Instantiate(Floor, GetDefaultPositionVector(coordinate, false), Quaternion.identity);
            return go;
        }
    }
}