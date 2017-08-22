using System;
using System.Collections;
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
        
        public void GetRequest(string url, Action<string> callback, Action<string> error) {
            var request = UnityWebRequest.Get(url);
            StartCoroutine(WaitForRequest(request, callback, error));
        }

        public void SendRoomUpdate(string url) {
            var request = UnityWebRequest.Post(url, "");
            MakeRequest(request, JsonUtility.ToJson(new Room {room = _instanceId}));
        }

        public void AuthRequest(string url, AuthData data, Action<string> callback, Action<string> error) {
            var request = UnityWebRequest.Post(url, "");
            MakeRequest(request,JsonUtility.ToJson(data), callback, error);
        }

        public void RegisterAsGuest(Action<string> callback, Action<string> error) {
            var request = UnityWebRequest.Post(NetworkConstants.RegisterAsGuest, "");
            StartCoroutine(WaitForRequest(request, callback, error));
        }

        public void Logout(Token token, Action<string> callback = null, Action<string> error = null) {
            var request = UnityWebRequest.Post(NetworkConstants.Logout, "");
            MakeRequest(request,JsonUtility.ToJson(token), callback, error);
        }

        public void GetUserData(string url, Token token, Action<string> callback, Action<string> error) {
            var request = UnityWebRequest.Post(url, "");
            MakeRequest(request, JsonUtility.ToJson(token), callback, error);
        }

        public void UpdateUser(User user) {
            var request = UnityWebRequest.Post(NetworkConstants.UpdateUser, "");
            MakeRequest(request, JsonUtility.ToJson(user));
        }

        private void MakeRequest(UnityWebRequest request, string json = null, Action<string> callback = null, Action<string> error = null) {
            if (json != null) {
                UploadHandler customUploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
                customUploadHandler.contentType = "application/json";
                request.uploadHandler = customUploadHandler;
            }
            StartCoroutine(WaitForRequest(request, callback, error));
        }

        private IEnumerator WaitForRequest(UnityWebRequest www, Action<string> callback = null,  Action<string> error = null) {
            yield return www.Send();
            
            if (www.isNetworkError) {
                error?.Invoke(JsonUtility.ToJson(new Error {error = "Network error, try again latter"}));
                yield break;
            }
            
            if (www.responseCode == 400) {
                error?.Invoke(www.downloadHandler.text);
            } else {
                callback?.Invoke(www.downloadHandler.text);
            }
        }
    }
}