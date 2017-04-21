using UnityEngine;
using System.Collections;

public class Buzz : MonoBehaviour {

	AudioSource source;

	void Start () {
		source = GetComponent<AudioSource>();
	}

	void LateUpdate () {
		float volume = Mathf.Sqrt(Mathf.Pow(Input.GetAxis("Mouse X"), 2) + Mathf.Pow(Input.GetAxis("Mouse Y"), 2)) / 10;
		source.volume = Mathf.Lerp(source.volume, volume, 0.3f);
	}

}
