namespace Constants {
    // Json Serialisation classes here
    public class JsonPort {
        public int port;
    }

    public class Room {
        public int room;
    }
    public class AuthData {
        public string password;
        public string username;
    }

    public class Error {
        public string error;
    }

    public class User {
        public bool isGuest;
        public string username;
        public int id;
        public string token;
        public int score;
        public ItemsInInventory[] itemsInInventories;

        public override string ToString() {
            return $"{username}  with {score} points has {itemsInInventories.Length} items";
        }
    }

    public class ItemsInInventory {
        public string itemName;
        public string itemCount;
    }

    public class Token {
        public string token;
    }
}