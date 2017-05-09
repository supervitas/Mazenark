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

        #region Auth

        public static string Register = string.Format("{0}/api/auth/register", ServerApiUrl);
        public static string Login = string.Format("{0}/api/auth/login", ServerApiUrl);
        public static string Logout = string.Format("{0}/api/auth/logout", ServerApiUrl);
        public static string Guest = string.Format("{0}/api/auth/guest", ServerApiUrl);

        #endregion

        #region User

        public static string UserByToken = string.Format("{0}/api/user/token", ServerApiUrl);
        public static string UserById = string.Format("{0}/api/user/id", ServerApiUrl);


        #endregion

        public static string GameResultUrl = string.Format("{0}/api/gameresult", ServerApiUrl);
    }
}