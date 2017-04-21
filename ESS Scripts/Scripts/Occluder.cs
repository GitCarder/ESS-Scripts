using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Occluder : MonoBehaviour {

	void Start ()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Occludable");
        foreach (GameObject go in gameObjects)
        {
            Transform[] transforms = go.GetComponentsInChildren<Transform>(true);
            foreach (Transform tf in transforms)
            {
                Renderer[] renderers = tf.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.sharedMaterial.SetInt("_ZWrite", 0);
                    renderer.sharedMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Greater);
                }
            }
        }
    }
}
