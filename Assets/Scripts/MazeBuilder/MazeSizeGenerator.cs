namespace MazeBuilder {
    public  class MazeSizeGenerator {
        private static MazeSizeGenerator instance;

        public static MazeSizeGenerator Instance {
            get { return instance ?? (instance = new MazeSizeGenerator()); }
        }
        private MazeSizeGenerator() {}

        public int X { get; private set; }

        public int Y { get; private set; }

        public  void GenerateFixedSize() {
            X = 97;
            Y = 97;
        }

    }
}