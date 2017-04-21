using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Screwdriver : ManipulatorTool {

    public Transform referenceBase;
    public Transform maleSocket;
	public Material defaultMaterial;
	public List<Transform> femaleSockets;
	public AudioClip plinkSound;
	private AudioSource audio;

	void Start() {
		audio = GetComponent<AudioSource>();
		Paint ();
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.GetComponentInParent<Manipulator>() != null)
			audio.PlayOneShot(plinkSound);
	}

	void Paint() {
		Transform[] children = transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform child in children) {
			Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers) {
				if (renderer.tag.Equals("Interactable")) continue;
				renderer.sharedMaterial = defaultMaterial;
			}
		}
	}
}
