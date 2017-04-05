using System;
using System.Linq;
using App.EventSystem;
using MazeBuilder;
using UnityEngine;
using UnityEngine.Networking;


namespace App.Server {
    public class MazeDelivery : NetworkBehaviour {
        private Maze _fetchedMaze;

        private Tile.Variant IntTileTypeToVariant(int type) {
            switch (type) {
                case 0:
                    return Tile.Variant.Empty;
                case 1:
                    return Tile.Variant.Room;
                case 2:
                    return Tile.Variant.Wall;
                default:
                    return Tile.Variant.Empty;
            }
        }

        private int VariantTyleTypeToInt(Tile.Variant type) {
            switch (type) {
                case Tile.Variant.Empty:
                    return 0;
                case Tile.Variant.Room:
                    return 1;
                case Tile.Variant.Wall:
                    return 2;
                default:
                    return 0;
            }
        }

        private Biome GetBiomeByName(string biomeName) {
            return Biome.AllBiomesList.First(bm => bm.Name == biomeName);
        }

        [ClientRpc]
        void RpcCreateMaze(int width, int hight) {
            _fetchedMaze = new Maze(width, hight, true);

        }

        [ClientRpc]
        void RpcFillMaze(int x, int y, string biomeName, int type) {
            _fetchedMaze[x, y].Biome = GetBiomeByName(biomeName);
            _fetchedMaze[x, y].Type = IntTileTypeToVariant(type);
        }

        [ClientRpc]
        void RpcMazeLoadingFinished(int width, int hight) {
            AppManager.Instance.MazeInstance = new MazeBuilder.MazeBuilder(width, hight, _fetchedMaze);
            AppManager.Instance.EventHub.CreateEvent("MazeLoaded", null);
            AppManager.Instance.EventHub.CreateEvent("mazedrawer:placement_finished", null);
        }


        public void GetMaze() {
            if (!isServer)
                return;

            var mazeInstance = AppManager.Instance.MazeInstance;
            var maze = mazeInstance.Maze;

            RpcCreateMaze(maze.Width, maze.Height); // create maze

            for (var x = 0; x < mazeInstance.Height; x++) {
                for (var y = 0; y < mazeInstance.Width; y++) {
                    RpcFillMaze(x, y, maze[x, y].Biome.Name, VariantTyleTypeToInt(maze[x, y].Type)); // fill maze
                }
            }

            RpcMazeLoadingFinished(maze.Width, maze.Height);
        }

    }
}