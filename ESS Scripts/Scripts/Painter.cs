using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Painter : MonoBehaviour {

    public Material material;

	void Start () {
        Transform[] children = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children) {
            Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                renderer.material = material;
            }
        }
    }
	
}
