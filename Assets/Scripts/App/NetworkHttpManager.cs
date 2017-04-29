using System.Collections;

using UnityEngine;

namespace App {
    public class NetworkHttpManager : MonoBehaviour {
        public static NetworkHttpManager Instance;

        private void Start() {
            if (Instance == null) {
                Instance = this;
            }
        }
        public IEnumerator CreateRequest(string url) {
            var www = new WWW(url);
            yield return www;

        }
    }
}