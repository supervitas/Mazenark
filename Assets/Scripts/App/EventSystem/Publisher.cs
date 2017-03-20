using System;
using System.Collections.Generic;
using System.Linq;

namespace App.EventSystem {
    public class Publisher {
        private readonly Dictionary <string, Dictionary<object, EventHandler<EventArguments>>> _eventHandlers =
            new Dictionary <string, Dictionary<object, EventHandler<EventArguments>>>();

        public void CreateEvent(string customEvent, EventArguments args) {
           Dictionary<object, EventHandler<EventArguments>> handlers;
            if (!_eventHandlers.TryGetValue(customEvent, out handlers)) return;
            List<EventHandler<EventArguments>> handlersList =
                new List<EventHandler<EventArguments>>(handlers.Values); // copy for protection if object change when event trigger
            foreach (var handler in handlersList) {
                if (handler ==  null) return;
                handler(this, args);
            }
        }

        public void Subscribe(string eventName, EventHandler<EventArguments> eventHandler, object subscriber) {
            if (!_eventHandlers.ContainsKey(eventName)) {
                _eventHandlers.Add(eventName, new Dictionary <object, EventHandler<EventArguments>>());
            }
             _eventHandlers[eventName].Add(subscriber, eventHandler);
        }

        public void Unsubscribe(string eventName, object unsubscriber) {
            Dictionary<object, EventHandler<EventArguments>> handlers;
            if (_eventHandlers.TryGetValue(eventName, out handlers)) {
                handlers.Remove(unsubscriber);
            }
        }

        public void UnsubscribeFromAll(object unsubscriber) {
            foreach (var handler in _eventHandlers.Values.ToList()) {
                foreach (var obj in handler.Keys.ToList()) {
                    if (obj == unsubscriber) {
                        handler.Remove(unsubscriber);
                    }
                }
            }
        }
    }
}