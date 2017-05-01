using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace App {
    public class NetworkHttpManager : MonoBehaviour {
        public static NetworkHttpManager Instance;

        private void Start() {
            if (Instance == null) {
                Instance = this;
            }
        }

        public UnityWebRequest GetRequest(string url, Action<string> callback) {
            var request = UnityWebRequest.Get(url);
            StartCoroutine(WaitForRequest(request, callback));
            return request;
        }

        private IEnumerator WaitForRequest(UnityWebRequest www, Action<string> callback) {
            yield return www.Send();

            if(www.isError) {
                Debug.LogError(www.error);
            } else {
                callback(www.downloadHandler.text);
            }

        }
    }
}