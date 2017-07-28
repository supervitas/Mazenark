using MazeBuilder.Utility;
using UnityEngine;

namespace Controls.Bosses {    
    public abstract class BasicBossControl : BasicEnemyControl {
        protected Room SpawnRoom;                                                                            
        protected Vector3[] RoomBounds = new Vector3[4];
        private  readonly Vector3 _zeroVector = Vector3.zero;

        
        public void SetSpawnRoom(Room room) {              
            SpawnRoom = room;            
            RoomBounds[0] = Utils.TransformToWorldCoordinate(new Coordinate(SpawnRoom.BottomLeftCorner.X - 1, SpawnRoom.BottomLeftCorner.Y + 1));            
            RoomBounds[1] = Utils.TransformToWorldCoordinate(new Coordinate(SpawnRoom.TopLeftCorner.X - 1, SpawnRoom.TopLeftCorner.Y - 1));
            RoomBounds[2] = Utils.TransformToWorldCoordinate(new Coordinate(SpawnRoom.BottomRightCorner.X + 1, SpawnRoom.BottomRightCorner.Y + 1));
            RoomBounds[3] = Utils.TransformToWorldCoordinate(new Coordinate(SpawnRoom.TopRightCorner.X + 1, SpawnRoom.TopRightCorner.Y - 1));
        }
        
        public Room GetSpawnRoom() { return SpawnRoom; }            

        protected bool TargetInRoom(Vector3 target) {                
            if (SpawnRoom == null) return false;
            
            return target.z < RoomBounds[0].z && target.z > RoomBounds[1].z &&  // left bottom and top left
                   target.x > RoomBounds[0].x && target.x < RoomBounds[2].x; // left bottom and bottom right
        }
        
        protected override bool CheckPlayersNear(out Vector3 playerTarget) {                        
            foreach (var target in _playersTransform) {
                if (target == null) continue;

                var distance = Vector3.Distance(transform.position, target.position);
                                
                if (distance <= EnemyAgroRange && TargetInRoom(target.position)) {
                    playerTarget = target.position;
                    return true;
                }
            }
            playerTarget = _zeroVector;           
            return false;
        }
       
    }
}
