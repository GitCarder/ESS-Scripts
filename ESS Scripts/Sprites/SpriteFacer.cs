using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SpriteFacer : MonoBehaviour {

	Material material;

	void Start() {
		material = GetComponent<Renderer>().sharedMaterial;
	}

	void LateUpdate() {
        if (Camera.current != null)
            transform.LookAt(Camera.current.transform);
	}

	public void SetActive(bool flag) {
		material.SetInt("_Active", flag ? 1 : 0);
	}

}
