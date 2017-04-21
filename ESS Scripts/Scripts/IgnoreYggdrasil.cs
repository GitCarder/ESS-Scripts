using UnityEngine;
using System.Collections;

public class IgnoreYggdrasil : MonoBehaviour {
	
	public Transform model;
	private Vector3 startScale;
	private Vector3 currentScale;

	void Start() {
		startScale = model.lossyScale;
	}

	void OnPreRender() {
		currentScale = model.localScale;
	}

	void OnPostRender() {		
	}

}
