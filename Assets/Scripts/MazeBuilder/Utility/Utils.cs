namespace MazeBuilder.Utility {
    public class Utils {
        // E.g. 0 → 4.5, 3 → 3*9 + 4.5
        public static float TransformToWorldCoordinate(int absoluteCoordinate) {
            return absoluteCoordinate * Constants.Maze.TILE_SIZE + Constants.Maze.TILE_SIZE / 2.0f;
        }
    }
}