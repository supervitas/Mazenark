using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.EventSystem {
    public class Publisher {
        private readonly Dictionary <string, EventHandler<EventArguments>> _eventHandlers =
            new Dictionary <string, EventHandler<EventArguments>>();

        public void CreateEvent(string customEvent) {
            EventHandler<EventArguments> handler;
            if (_eventHandlers.TryGetValue(customEvent, out handler)) {
                if (handler ==  null) return;
                handler(this, new EventArguments(customEvent));
            }
        }

        public void Subscribe(string eventName, EventHandler<EventArguments> Event) {
            if (!_eventHandlers.ContainsKey(eventName)) {
                _eventHandlers.Add(eventName, Event);
            } else {
                 _eventHandlers[eventName] += Event;
            }
        }

        public void Unsubscribe(string eventName, EventHandler<EventArguments> Event) {
            EventHandler<EventArguments> handler;
            if (_eventHandlers.TryGetValue(eventName, out handler)) {
                if (handler ==  null) return;

            }
        }
    }
}