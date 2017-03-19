using System;
using System.Collections.Generic;

namespace App.EventSystem {
    public class Publisher {
        private readonly Dictionary <string, Dictionary<object, EventHandler<EventArguments>>> _eventHandlers =
            new Dictionary <string, Dictionary<object, EventHandler<EventArguments>>>();

        public void CreateEvent(string customEvent) {
           Dictionary<object, EventHandler<EventArguments>> handlers;
            if (_eventHandlers.TryGetValue(customEvent, out handlers)) {
                List<EventHandler<EventArguments>> handlersList =
                    new List<EventHandler<EventArguments>>(handlers.Values); // copy for protection if object change when event trigger
                foreach (var handler in handlersList) {
                    if (handler ==  null) return;
                    handler(this, new EventArguments(customEvent));
                }
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
    }
}