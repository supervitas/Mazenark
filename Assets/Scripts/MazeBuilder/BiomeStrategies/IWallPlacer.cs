namespace MazeBuilder.BiomeStrategies {
    public interface IWallPlacer{
        // stateless!
        Maze PlaceWalls(Maze roomedMaze);
    }
}


