namespace App {
    public class MazeSizeGenerator {

        public int X { get; private set; }

        public int Y { get; private set; }

        public  void GenerateFixedSize() {
            X = 65;
            Y = 65;
        }
    }
}