using System;
using UnityEngine;

namespace App.EventSystem {
    public class Publisher {
        // Declare the event using EventHandler<T>
        public event EventHandler<EventArguments> RaiseCustomEvent;

        public void DoSomething(string customEvent) {
            OnRaiseCustomEvent(new EventArguments(customEvent));
        }

        // Wrap event invocations inside a protected virtual method
        // to allow derived classes to override the event invocation behavior
        protected virtual void OnRaiseCustomEvent(EventArguments e) {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<EventArguments> handler = RaiseCustomEvent;
            Debug.Log(handler);
            // Event will be null if there are no subscribers
            if (handler != null) {
                handler(this, e);
            }
        }



    }
}