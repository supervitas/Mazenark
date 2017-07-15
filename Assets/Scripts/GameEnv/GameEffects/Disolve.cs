using System.Collections;
using System.Linq;
using UnityEngine;

namespace GameEnv.GameEffects {    
    public class Disolve : MonoBehaviour {        
        private float _currentValue = 0f;        
        
        public void BeginDisolve (float time = 2f) {                       
                        
            foreach (var mat in GetComponentsInChildren<Renderer>().Select(render => render.material)) {
                
                if (!mat.HasProperty("_SliceAmount")) {
                    Debug.LogError(string.Format("{0} gameobject with {1} dosent have disolve shader in material",
                        gameObject.name, mat.name));
                    continue;
                }
                
                StartCoroutine(StartDisolve(mat));                
                
            }
        }

        private IEnumerator StartDisolve(Material mat) {                                              
            for (var i = 0; i < 20; i++) {
                _currentValue += 0.05f;                
                mat.SetFloat("_SliceAmount", _currentValue);               
                yield return new WaitForSeconds(0.1f);
            }
        }   
    }
}