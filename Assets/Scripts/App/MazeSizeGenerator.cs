using UnityEngine;

namespace App {
    public class MazeSizeGenerator {

        public int X { get; private set; }

        public int Y { get; private set; }

        public void GenerateFixedSize(int size) {
            X = size;
            Y = size;
        }

        public void GenerateRndSize() {
            var rndSize = Random.Range(21, 40 / 2 ) * 2 + 1;
            X = rndSize;
            Y = rndSize;
        }
    }
}