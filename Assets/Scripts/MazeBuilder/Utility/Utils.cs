using UnityEngine;

namespace MazeBuilder.Utility {
    public class Utils {
        // E.g. 0 → 4.5, 3 → 3*9 + 4.5
        public static float TransformToWorldCoordinate(int absoluteCoordinate) {
            return absoluteCoordinate * global::Constants.Maze.TILE_SIZE + global::Constants.Maze.TILE_SIZE / 2.0f;
        }

        public static float TransformToWorldCoordinate(float absoluteCoordinate) {
            return absoluteCoordinate * global::Constants.Maze.TILE_SIZE + global::Constants.Maze.TILE_SIZE / 2.0f;
        }

        public static Vector3 TransformToWorldCoordinate(Coordinate coord, float height = 0.1f) {
            return new Vector3(TransformToWorldCoordinate(coord.X), height, TransformToWorldCoordinate(coord.Y));
        }

        public static Coordinate TransformWorldToLocalCoordinate(float x, float z) {
            return new Coordinate((int) x / global::Constants.Maze.TILE_SIZE, (int) z / global::Constants.Maze.TILE_SIZE);
        }

        public static Vector3 GetDefaultPositionVector(Coordinate coords, float y = 0f) {
	        return new Vector3 {
	            x = TransformToWorldCoordinate(coords.X),
	            y = y,
	            z = TransformToWorldCoordinate(coords.Y)
	        };
	    }
    }
}