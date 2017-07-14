using System.Collections;

using UnityEngine;

namespace GameEnv.GameEffects {
    public class Disolve : MonoBehaviour {
        private float _currentValue = 0f;
        
        public void BeginDisolve (float time = 2f) {
            var materials = GetComponentInChildren<Renderer>().materials;
            foreach (var mat in materials) {
                
                if (!mat.HasProperty("_SliceAmount")) {
                    Debug.LogError(string.Format("{0} gameobject with {1} dosent have disolve shader in material",
                        gameObject.name, mat.name));
                    continue;
                }
                
                StartCoroutine(BeginDisolve(mat, time));                
                
            }
        }

        private IEnumerator BeginDisolve(Material mat, float time) {
            yield return new WaitForSeconds(0.5f);
            
            var disolveValue = 0.1f / (time - 0.5f);
            
            for (float f = 0f; f <= time; f += 0.1f) {
                _currentValue += disolveValue;                
                mat.SetFloat("_SliceAmount", _currentValue);               
                yield return new WaitForSeconds(0.1f);
            }
        }        
    }
}