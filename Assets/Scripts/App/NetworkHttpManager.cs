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

        public UnityWebRequest GetRequest(string url, Action<string> callback) {
            var request = UnityWebRequest.Get(url);
            StartCoroutine(WaitForRequest(request, callback));
            return request;
        }

        public void PlayerLeftFromRoom() {
            var request = UnityWebRequest.Post(NetworkConstants.GamePlayerLeft,
                JsonUtility.ToJson(new Room {room = _instanceId}));
            StartCoroutine(WaitForRequest(request, null));
        }

        private IEnumerator WaitForRequest(UnityWebRequest www, Action<string> callback) {
            yield return www.Send();

            if(www.isError) {
                Debug.LogError(www.error);
            } else {
                if (callback != null) {
                    callback(www.downloadHandler.text);
                }
            }
        }
    }
}