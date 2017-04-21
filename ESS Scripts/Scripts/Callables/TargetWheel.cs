using UnityEngine;
using System.Collections;

public class TargetWheel : Callable {

    public Transform referenceBase;
    public Transform referenceOrigin;

    private Vector3 startPosition;
    private Quaternion startRotation;

	void Start() {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }
	
	void LateUpdate() {
	}

    public override IEnumerator Call(string method, string[] arguments, bool block) {
        switch (method) {
            case "RESET":
                transform.parent = referenceBase;
                transform.localPosition = startPosition;
                transform.localRotation = startRotation;
                break;
            default:
                yield return null;
                break;
        }
    }

}
