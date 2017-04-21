using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ZoomTarget : MonoBehaviour {

	public Window3D window;

	void Update() {
		float p = Mathf.Max(transform.localScale.x / window.transform.localScale.x,
		          Mathf.Max(transform.localScale.y / window.transform.localScale.y,
		                    transform.localScale.z / window.transform.localScale.z));
			transform.localScale = new Vector3(p * window.transform.localScale.x,
			                                   p * window.transform.localScale.y,
															           p * window.transform.localScale.z);
	}

}
