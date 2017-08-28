namespace Constants {
    public class NetworkConstants {
		// ↓ Production
		public static string GameRoomAdress = "84.23.37.194";
		public static string ServerApiUrl = "http://mazenark.tk";

		// ↓ Debug
		// public static string GameRoomAdress = "localhost"; // change to ip
    //    public static string ServerApiUrl = "http://localhost:7000"; // change to domain
        
        public static string UnityServerOnlyUrl = "http://localhost:7000"; // DONT CHANGE

        #region Room

        public static string RoomGetRoom = $"{ServerApiUrl}/api/getRoom";

        public static string RoomPlayerLeft = $"{UnityServerOnlyUrl}/api/room/playerLeft";
        public static string RoomPlayerJoined = $"{UnityServerOnlyUrl}/api/room/playerJoined";
        public static string RoomGameStarted = $"{UnityServerOnlyUrl}/api/room/gameStarted";
        public static string RoomGameEnded = $"{UnityServerOnlyUrl}/api/room/gameEnded";

        #endregion

        #region Auth

        public static string Register = $"{ServerApiUrl}/api/auth/register";
        public static string Login = $"{ServerApiUrl}/api/auth/login";
        public static string Logout = $"{ServerApiUrl}/api/auth/logout";
        public static string RegisterAsGuest = $"{ServerApiUrl}/api/auth/guest";

        #endregion

        #region User

        public static string UserByToken = $"{ServerApiUrl}/api/user/token";
        public static string UserById = $"{ServerApiUrl}/api/user/id";
	    public static string UpdateUser = $"{ServerApiUrl}/api/user/updateData";

        #endregion
    }
}