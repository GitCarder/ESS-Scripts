using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class HOFF9000 : Callable {

	private AudioSource source;
	private float[] samples = new float[1024];
	private float targetVolume = 0;
	private Dictionary<string, AudioClip> tutorial = new Dictionary<string, AudioClip>();

	void Start() {
		source = GetComponent<AudioSource>();
		LoadTutorial();
	}

	void LoadTutorial() {
		string[] filePaths = Directory.GetFiles (Application.dataPath + "/Resources/Tutorial", "*.wav");
		foreach (string filePath in filePaths) {
			int dirSepIx = filePath.LastIndexOf (Path.DirectorySeparatorChar);
			string fileName = filePath.Substring(dirSepIx + 1, filePath.Length - 5 - dirSepIx);
			AudioClip audioClip = Resources.Load<AudioClip>("Tutorial/" + fileName);
			tutorial.Add(fileName, audioClip);
		}
	}
	
	public override IEnumerator Call(string method, string[] arguments, bool block) {
		switch (method) {
		case "TUTORIAL":
			source.clip = tutorial[arguments[0]];
			source.Play();
			while (block && source.isPlaying)
				yield return new WaitForFixedUpdate();
			break;
		case "MOVE_TO":
			Transform target = GameObject.Find(arguments[0]).transform;
			float seconds = arguments.Length >= 2 ? float.Parse(arguments[1]) : 3;
			if (block) {
				yield return StartCoroutine(MoveTo(target, seconds));
			} else {
				StartCoroutine(MoveTo(target, seconds));
			}
			break;
		}
	}
	
	IEnumerator MoveTo(Transform target, float seconds) {
		Vector3 from = transform.position;
		Vector3 to = target.position;

		for (float t = 0.0f; t <= 1.0f; t = Mathf.Min(1, t + Time.deltaTime / seconds))
		{
			transform.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0, 1, t));
			if (t == 1.0f) {
				// just to make sure that t=1 is included
				break;
			}
			yield return new WaitForFixedUpdate();
		}
	}

	void LateUpdate() {
		if (source.clip != null) {
			source.clip.GetData(samples, Mathf.Max(source.timeSamples - samples.Length, 0));
			float volume = 0;
			for (int i = 0; i < source.clip.channels; i++) {
				float sum = 0;
				for (int j = 0; j < samples.Length / source.clip.channels; j++) {
					sum += Mathf.Pow(samples[j * source.clip.channels + i], 2);
				}
				volume += Mathf.Sqrt(sum / samples.Length);
			}
			targetVolume = volume;
		}
	}
	
	void Update() {
		transform.localScale = Vector3.Lerp (transform.localScale, Vector3.one * (1 + targetVolume * 3), 0.5f);
	}

}
