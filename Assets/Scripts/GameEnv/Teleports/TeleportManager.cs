using System.Collections.Generic;
using MazeBuilder;
using MazeBuilder.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace GameEnv.Teleports {
    public class TeleportManager : NetworkBehaviour {

        [SerializeField] private GameObject TeleportGo;
        
        private readonly List<Teleport> _teleports = new List<Teleport>();
        private readonly Maze _maze = App.AppManager.Instance.MazeInstance.Maze;
        
        public void CreateTeleports(int count = 3) {
            for (var i = 0; i < count; i++) {
                var enterTeleportPosition = GetTeleportPosition();
                var exitTeleportPosition = GetTeleportPosition();               
                
                var enterTeleport = Instantiate(TeleportGo, enterTeleportPosition, Quaternion.identity);
                var exitTeleport = Instantiate(TeleportGo, exitTeleportPosition, Quaternion.identity);

                var enterTp = enterTeleport.GetComponent<Teleport>();
                var exitTp = exitTeleport.GetComponent<Teleport>();
                enterTp.SetTeleportTo(exitTp);
                exitTp.SetTeleportTo(enterTp);

                NetworkServer.Spawn(enterTeleport);
                NetworkServer.Spawn(exitTeleport);
            }
        }
        
        private Vector3 GetTeleportPosition() {
            var count = _maze.Biomes.Count;
            
            while (true) {                
                var randBiome = Random.Range(0, count);
                             
                var randTile = Random.Range(0, _maze.Biomes[randBiome].tiles.Count);                

                if (_maze.Biomes[randBiome].tiles[randTile].Type == Tile.Variant.Empty &&
                    _maze.Biomes[randBiome].tiles[randTile].Biome != Biome.Spawn && 
                    _maze.Biomes[randBiome].tiles[randTile].Biome != Biome.Safehouse) {
                    
                    return Utils.GetDefaultPositionVector(_maze.Biomes[randBiome].tiles[randTile].Position, 2f);
                }
            }            
        }
    }
}