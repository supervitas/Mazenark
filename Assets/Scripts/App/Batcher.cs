using System.Collections.Generic;
using UnityEngine;

namespace App {
    public class Batcher {
        private static  Batcher instance;

        public static Batcher Instance {
            get { return instance ?? (instance = new Batcher()); }
        }

        private Batcher(){}

        public GameObject Batch(GameObject[] children, string name) {
            var parent = new GameObject {name = name, isStatic = true};
            foreach (var child in children) {
                child.transform.parent = parent.transform;
            }
            StaticBatchingUtility.Combine(parent.gameObject);
            return parent;
        }

        public void BatchByDivider(int objectsPerBatch, GameObject[] gos, string name) {
            for (var i = 0; i < gos.Length; i += objectsPerBatch) {
                var gosList = new List<GameObject>();
                var limit = objectsPerBatch < gos.Length ? objectsPerBatch : gos.Length;
                for (var j = i; j < limit; j++) {
                    gosList.Add(gos[j]);
                }
                Batch(gosList.ToArray(), name + i);
            }
        }
    }
}
