using System;

namespace App.EventSystem {
    public class EventArguments : EventArgs {
        public EventArguments(string s) {
            Message = s;
        }

        public string Message { get; set; }
    }
}
