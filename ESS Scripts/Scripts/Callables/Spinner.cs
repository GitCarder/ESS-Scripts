using UnityEngine;
using System.Collections;

public class Spinner : Callable {

    public Transform referenceBase;
    private Transform attached;
    private Vector3 originalEulerAngles;
	private AudioSource audiosource;

    void Start() {
		originalEulerAngles = transform.localEulerAngles;
		audiosource = GetComponent<AudioSource>();
    }
    
    public override IEnumerator Call(string method, string[] arguments, bool block) {
        switch (method) {
            case "RESET":
                transform.localEulerAngles = originalEulerAngles;
                break;
            case "SPIN":
                // arguments: [degrees]
                float degrees = float.Parse(arguments[0]);
                float seconds = arguments.Length == 2 ? float.Parse(arguments[1]) : 2.0f;

                if (block) {
                    yield return StartCoroutine(Spin(degrees, seconds));
                } else {
                    StartCoroutine(Spin(degrees, seconds));
                }
                break;
            case "ATTACH":
                if (attached != null)
                    attached.parent = referenceBase;
                attached = GameObject.Find(arguments[0]).transform;
                attached.parent = transform;
                break;
            case "DEATTACH":
                attached.parent = referenceBase;
                attached = null;
                break;
            default:
                yield return null;
                break;
        }
    }


    IEnumerator Spin(float degrees, float seconds) {
        Vector3 startEulerAngles = transform.localEulerAngles;
        Vector3 endEulerAngles = startEulerAngles + Vector3.forward * degrees;

        for (float t = 0.0f; t <= 1.0f; t = Mathf.Min(1, t + Time.deltaTime / seconds)) {
            transform.localEulerAngles = Vector3.Lerp(startEulerAngles, endEulerAngles, Mathf.SmoothStep(0, 1, t));
			
			audiosource.volume = Mathf.Sin (t * Mathf.PI) / 2;
			audiosource.pitch = 0.75f + audiosource.volume / 4;

            if (t == 1.0f) {
                // just to make sure that t=1 is included
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
