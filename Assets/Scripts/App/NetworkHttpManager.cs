using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Lobby;
using UnityEngine;
using UnityEngine.Networking;

namespace App {
    public class NetworkHttpManager : MonoBehaviour {
        public static NetworkHttpManager Instance;
        private int _instanceId;

        private void Start() {
            if (Instance == null) {
                Instance = this;
            }
            _instanceId = FindObjectOfType<LobbyManager>().InstanceId;
        }

        public UnityWebRequest GetRequest(string url, Action<string> callback, Action<string> error) {
            var request = UnityWebRequest.Get(url);
            StartCoroutine(WaitForRequest(request, callback, error));
            return request;
        }

        public void PlayerLeftFromRoom() {
            var request = UnityWebRequest.Post(NetworkConstants.GamePlayerLeft,
                JsonUtility.ToJson(new Room {room = _instanceId}));
            request.uploadHandler.contentType= "application/json";
            StartCoroutine(WaitForRequest(request, null, null));
        }

        private IEnumerator WaitForRequest(UnityWebRequest www, Action<string> callback,  Action<string> error) {
            yield return www.Send();
            if(www.responseCode == 400 && error != null) {
                error(www.downloadHandler.text);
            } else {
                if (callback != null) {
                    callback(www.downloadHandler.text);
                }
            }
        }
    }
}