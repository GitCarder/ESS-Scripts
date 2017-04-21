using UnityEngine;
using System.Collections;

public class TargetWheelCrane : Callable {

    public Transform targetWheel;
    public Transform targetWheelClaw;
    public Transform targetWheelClawPosition;
    public Transform targetWheelIntermediaryPosition;
    private Vector3 targetWheelClawStartPosition;
    private float defaultSeconds = 4.0f;
	private AudioSource audiosource;

    void Start() {
        targetWheelClawStartPosition = transform.InverseTransformPoint(targetWheelClaw.position);
		audiosource = GetComponent<AudioSource>();
    }

    public override IEnumerator Call(string method, string[] arguments, bool block) {
        switch (method) {
            case "LOWER":
                // arguments: <opt.: SECONDS>

                float seconds = (arguments.Length >= 1) ? float.Parse(arguments[0]) : defaultSeconds;

                if (block) {
                    yield return StartCoroutine(Lower(seconds));
                } else {
                    StartCoroutine(Lower(seconds));
                }
                break;
            case "RAISE":
                // arguments: <opt.: SECONDS>

                seconds = (arguments.Length >= 1) ? float.Parse(arguments[0]) : defaultSeconds;

                if (block) {
                    yield return StartCoroutine(Raise(seconds));
                } else {
                    StartCoroutine(Raise(seconds));
                }
                break;
        }
    }

    private IEnumerator Lower(float seconds) {
        Vector3 from = transform.InverseTransformPoint(targetWheel.position);
        Vector3 to = transform.InverseTransformPoint(targetWheelIntermediaryPosition.position);

        for (float t = 0; t < 1; t += Time.deltaTime / seconds) {
            targetWheel.position = transform.TransformPoint(Vector3.Lerp(from, to, Mathf.SmoothStep(0, 1, t)));
            targetWheelClaw.position = targetWheelClawPosition.position;
			ChangeSound(t);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Raise(float seconds) {
        Vector3 from = transform.InverseTransformPoint(targetWheelClaw.position);
        Vector3 to = targetWheelClawStartPosition;

        for (float t = 0; t < 1; t += Time.deltaTime / seconds) {
            targetWheelClaw.position = transform.TransformPoint(Vector3.Lerp(from, to, Mathf.SmoothStep(0, 1, t)));
			ChangeSound(t);
            yield return new WaitForFixedUpdate();
        }
    }

	void ChangeSound(float t) {
		audiosource.volume = Mathf.Sin (t * Mathf.PI) / 2;
		audiosource.pitch = 0.75f + audiosource.volume / 4;
	}

}
