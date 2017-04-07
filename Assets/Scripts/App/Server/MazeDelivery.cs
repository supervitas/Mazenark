using System.Collections.Generic;
using System.Linq;
using MazeBuilder;
using UnityEngine.Networking;
using MazeBuilder.BiomeStrategies;
using UnityEngine;

namespace App.Server {
    public class MazeDelivery : NetworkBehaviour {
        private Maze _fetchedMaze;

        public struct MazeStruct {
             public int X;
             public int Y;
             public string BiomeName;
             public int TileType;
			 public int BiomeInstanceId;

            public MazeStruct(int x, int y, string biomeName, int tileType, int biomeInstanceId) {
                X = x;
                Y = y;
                BiomeName = biomeName;
                TileType = tileType;
				BiomeInstanceId = biomeInstanceId;
            }
        }

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
        public void RpcCreateMaze(int width, int hight) {
            _fetchedMaze = new Maze(width, hight, true);

        }

        [ClientRpc]
        private void RpcFillMaze(MazeStruct[] mazeArr) {
            foreach (var tile in mazeArr) {
                _fetchedMaze[tile.X, tile.Y].Biome = GetBiomeByName(tile.BiomeName);
                _fetchedMaze[tile.X, tile.Y].Type = IntTileTypeToVariant(tile.TileType);
				_fetchedMaze[tile.X, tile.Y].BiomeID = tile.BiomeInstanceId;
			}
//			AdvancedBiomePlacer.WriteBiomesListIntoMaze(100, _fetchedMaze);
        }

        [ClientRpc]
        public void RpcMazeLoadingFinished(int width, int hight) {
            AppManager.Instance.MazeInstance = new MazeBuilder.MazeBuilder(width, hight, _fetchedMaze);
            AppManager.Instance.EventHub.CreateEvent("MazeLoaded", null);
            AppManager.Instance.EventHub.CreateEvent("mazedrawer:placement_finished", null);
        }

        public void GetMaze() {
            if (!isServer)
                return;

            var messageBatchSize = 10; // how much rows will be send in one message;
            var counter = 0;
            var biomeList = new List<MazeStruct>();

            var mazeInstance = AppManager.Instance.MazeInstance;
            var maze = mazeInstance.Maze;


            RpcCreateMaze(maze.Width, maze.Height); // create maze

            for (var x = 0; x < mazeInstance.Height; x++) {
                for (var y = 0; y < mazeInstance.Width; y++) {
                    biomeList.Add(new MazeStruct(x, y, maze[x, y].Biome.Name,
                        VariantTyleTypeToInt(maze[x, y].Type), maze[x, y].BiomeID)); // fill maze
                }
                counter++;
                if (counter >= messageBatchSize) {
                    RpcFillMaze(biomeList.ToArray());
                    counter = 0;
                    biomeList.Clear();
                }
            }
            RpcFillMaze(biomeList.ToArray()); // send final chunk of data

            RpcMazeLoadingFinished(maze.Width, maze.Height);
        }

    }
}