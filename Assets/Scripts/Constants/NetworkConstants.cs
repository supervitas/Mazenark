namespace Constants {
    public class NetworkConstants {
        public static string GameRoomAdress = "localhost";
        public static string ServerApiUrl = "http://localhost:7000";

        #region Room
        public static string RoomGetRoom = string.Format("{0}/api/getRoom", ServerApiUrl);
        public static string RoomPlayerLeft = string.Format("{0}/api/room/playerLeft", ServerApiUrl);
        public static string RoomPlayerJoined = string.Format("{0}/api/room/playerJoined", ServerApiUrl);
        public static string RoomGameStarted = string.Format("{0}/api/room/gameStarted", ServerApiUrl);
        public static string RoomGameEnded = string.Format("{0}/api/room/gameEnded", ServerApiUrl);
        #endregion

        public static string GameResultUrl = string.Format("{0}/api/gameresult", ServerApiUrl);
    }
}