using System.Collections;
using System.Linq;
using UnityEngine;

namespace GameEnv.GameEffects {    
    public class Disolve : MonoBehaviour {
        private static float _waitTime = 0.05f;
        private float _currentValue = 0f;
        
        public void BeginDisolve (float time = 1.2f) {            
            foreach (var materials in GetComponentsInChildren<Renderer>().Select(render => render.materials)) {
                foreach (var mat in materials) {
                    if (!mat.HasProperty("_SliceAmount")) {
                        Debug.LogError(
                            $"{gameObject.name} gameobject with {mat.name} dosent have disolve shader in material");
                        continue;
                    }
                    StartCoroutine(StartDisolve(mat, time));
                }
            }
        }

        private IEnumerator StartDisolve(Material mat, float time) {            
            var slice = Shader.PropertyToID("_SliceAmount");

            var cycle = time * 10;     
            var dissolveValue = 1 / cycle;

            for (var i = 0; i < (int) (cycle / (_waitTime * 10)); i++) {
                _currentValue += dissolveValue;
                mat.SetFloat(slice, _currentValue);               
                yield return new WaitForSeconds(_waitTime);
            }
        }   
    }
}