namespace MazeBuilder.BiomeStrategies {
    public class EmptyRoomPlacer : IRoomPlacer{
        private static readonly EmptyRoomPlacer instance = new EmptyRoomPlacer();

        private EmptyRoomPlacer(){}
        public static IRoomPlacer Instance{
            get{
                return instance;
            }
        }

        public Maze PlaceRoom(Maze maze, int x, int y, int chunkLeftBoundary,
            int chunkRightBoundary, int chunkTopBoundary, int chunkBottomBoundary) {
            return maze;
        }
    }
}
