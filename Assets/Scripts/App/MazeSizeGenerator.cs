namespace App {
    public class MazeSizeGenerator {

        public int X { get; private set; }

        public int Y { get; private set; }

        public  void GenerateFixedSize() {
            X = 97;
            Y = 97;
        }
    }
}