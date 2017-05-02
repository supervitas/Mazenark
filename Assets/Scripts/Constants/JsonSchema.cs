using System;

namespace Constants {
    // Json Serialisation classes here
    public class JsonPort {
        public int port;
    }

    [Serializable]
    public class Room {
        public int room;
    }

    public class Error {
        public string error;
    }
}