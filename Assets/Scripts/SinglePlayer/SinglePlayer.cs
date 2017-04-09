using System.Collections;
using System.Collections.Generic;
using App;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SinglePlayer {
    public class SinglePlayer : MonoBehaviour {
        public GameObject Player;
        private Vector3 _position;
        public void StartSinglePlayer(Vector3 spawnPosition) {
            SceneManager.LoadScene("Maze Scene");
            SceneManager.sceneLoaded += OnSceneLoaded;
            _position = spawnPosition;

        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            Instantiate(Player, _position, Quaternion.identity);
        }

        IEnumerator LoadGameLevel(){
            AsyncOperation async = SceneManager.LoadSceneAsync("Maze Scene", LoadSceneMode.Single);
            async.allowSceneActivation = false;
            while(!async.isDone) {
                yield return null;
            }
            async.allowSceneActivation = true;
        }

    }


}