namespace Constants {
    public class NetworkConstants {
        public static string GameRoomAdress = "localhost"; // change to ip
        public static string ServerApiUrl = "http://localhost:7000"; // change to domen
        public static string UnityServerOnlyUrl = "http://localhost:7000"; // DONT CHANGE

        #region Room

        public static string RoomGetRoom = string.Format("{0}/api/getRoom", ServerApiUrl);

        public static string RoomPlayerLeft = string.Format("{0}/api/room/playerLeft", UnityServerOnlyUrl);
        public static string RoomPlayerJoined = string.Format("{0}/api/room/playerJoined", UnityServerOnlyUrl);
        public static string RoomGameStarted = string.Format("{0}/api/room/gameStarted", UnityServerOnlyUrl);
        public static string RoomGameEnded = string.Format("{0}/api/room/gameEnded", UnityServerOnlyUrl);

        #endregion

        #region Auth

        public static string Register = string.Format("{0}/api/auth/register", ServerApiUrl);
        public static string Login = string.Format("{0}/api/auth/login", ServerApiUrl);
        public static string Logout = string.Format("{0}/api/auth/logout", ServerApiUrl);
        public static string RegisterAsGuest = string.Format("{0}/api/auth/guest", ServerApiUrl);

        #endregion

        #region User

        public static string UserByToken = string.Format("{0}/api/user/token", ServerApiUrl);
        public static string UserById = string.Format("{0}/api/user/id", ServerApiUrl);


        #endregion

        public static string GameResultUrl = string.Format("{0}/api/gameresult", UnityServerOnlyUrl);
    }
}