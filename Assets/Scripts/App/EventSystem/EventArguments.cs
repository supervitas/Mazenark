using System;
using UnityEngine;

namespace App.EventSystem {
    public class EventArguments : EventArgs {
        public EventArguments(string s) {
            Message = s;
        }
        public EventArguments(Transform transform, string biomeName) {
            Transform = transform;
            BiomeName = biomeName;
        }
        public string Message { get; set; }
        public string BiomeName { get; set; }
        public Transform Transform { get; set; }
    }
}
