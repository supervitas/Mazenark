namespace Constants {
    public class NetworkConstants {
        public static string GameRoomAdress = "localhost";
        public static string ServerApiUrl = "http://localhost:9000";

        public static string GameResultUrl = string.Format("{0}/api/gameresult", ServerApiUrl);
        public static string GameGetRoom = string.Format("{0}/api/getRoom", ServerApiUrl);
        public static string GamePlayerLeft = string.Format("{0}/api/room/playerLeft", ServerApiUrl);
        public static string GameStarted = string.Format("{0}/api/gameStarted", ServerApiUrl);
        public static string GameEnded = string.Format("{0}/api/gameEnded", ServerApiUrl);
    }
}