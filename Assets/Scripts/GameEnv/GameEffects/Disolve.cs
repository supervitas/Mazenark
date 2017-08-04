using System.Collections;
using System.Linq;
using UnityEngine;

namespace GameEnv.GameEffects {    
    public class Disolve : MonoBehaviour {        
        private float _currentValue = 0f;        
        
        public void BeginDisolve (float time = 2f) {            
            foreach (var materials in GetComponentsInChildren<Renderer>().Select(render => render.materials)) {
                foreach (var mat in materials) {
                    if (!mat.HasProperty("_SliceAmount")) {
                        Debug.LogError(
                            $"{gameObject.name} gameobject with {mat.name} dosent have disolve shader in material");
                        continue;
                    }
                    StartCoroutine(StartDisolve(mat));
                }
            }
        }

        private IEnumerator StartDisolve(Material mat) {            
            var slice = Shader.PropertyToID("_SliceAmount");
            
            for (var i = 0; i < 20; i++) {
                _currentValue += 0.05f;                
                mat.SetFloat(slice, _currentValue);               
                yield return new WaitForSeconds(0.02f);
            }
        }   
    }
}