using UnityEngine;
using System.Collections;

public class Beeper : Callable {

	public AudioClip notificationSound;
	public AudioClip tadaaSound;
	public AudioClip messageSound;

	private AudioSource audiosource;

	void Start() {
		audiosource = GetComponent<AudioSource>();
	}

	public override IEnumerator Call(string method, string[] arguments, bool block) {
		switch (method) {
		case "BEEP":
			audiosource.PlayOneShot(notificationSound);
			yield return null;
			break;
		case "TADAA":
			audiosource.PlayOneShot(tadaaSound);
			yield return null;
			break;
		case "MESSAGE":
			audiosource.PlayOneShot(messageSound);
			yield return null;
			break;
		}
	}

}
