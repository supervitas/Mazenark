using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App {
    public class NetworkHttpManager : MonoBehaviour {
        public static NetworkHttpManager Instance;

        private void Start() {
            if (Instance == null) {
                Instance = this;
            }
        }

        public WWW MakeRequest(string url) {
            var www = new WWW(url);
            StartCoroutine(WaitForRequest(www));
            return www;
        }
        public WWW POST(string url, Dictionary<string, string> post) {
            WWWForm form = new WWWForm();
            foreach (KeyValuePair<String, String> post_arg in post)
            {
                form.AddField(post_arg.Key, post_arg.Value);
            }
            WWW www = new WWW(url, form);

            StartCoroutine(WaitForRequest(www));
            return www;
        }


        private IEnumerator WaitForRequest(WWW www) {
            yield return www;
            if (www.error != null) {
                Debug.Log("WWW Error: " + www.error);
            }
        }
    }
}