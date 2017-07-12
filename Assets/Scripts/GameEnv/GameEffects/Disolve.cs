using System.Collections;

using UnityEngine;

namespace GameEnv.GameEffects {
    public class Disolve : MonoBehaviour {
        private float _currentValue = 0f;
        
        public void BeginDisolve (float time = 2f) {
            var materials = GetComponentInChildren<Renderer>().materials;
            foreach (var mat in materials) {
                if (!mat.HasProperty("_SliceAmount")) {
                    Debug.LogError(string.Format("{0} gameobject and {1} dosent have disolve shader in material",
                        gameObject.name, mat.name));
                    continue;
                }
                
                StartCoroutine(Disolving(mat, time));                
                
            }
        }

        private IEnumerator Disolving(Material mat, float time) {
            for (float f = 0f; f <= time; f += 0.1f) {
                mat.SetFloat("_SliceAmount", f);
                yield return new WaitForSeconds(.1f);
            }
        }        
    }
}