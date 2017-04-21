using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Assistant : MonoBehaviour {

    private LineRenderer lineRenderer;
    private int length = 20;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(Color.yellow, Color.red);
        lineRenderer.SetWidth(0.2f, 0.2f);
        lineRenderer.SetVertexCount(length);
    }
	
	void LateUpdate() {
        for (int i = 0; i < length; i++) {
            lineRenderer.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + Time.time), 0));
        }
    }

}
