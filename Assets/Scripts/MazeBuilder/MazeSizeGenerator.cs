namespace Constants {
    public  class MazeSizeGenerator {
        private static MazeSizeGenerator instance;

        private MazeSizeGenerator() {}

        public static MazeSizeGenerator Instance{
            get { return instance ?? (instance = new MazeSizeGenerator()); }
        }
        public int _x { get; private set; }

        public int _y { get; private set; }

        public  void generateFixedSize() {
            _x = 64;
            _y = 64;
        }

    }
}