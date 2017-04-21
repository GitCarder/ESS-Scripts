using UnityEngine;
using System.Collections;

public class Saw : Callable {
    

    public Transform referenceOrigin;

    public Transform wagon;
    public Transform mount;
    public Transform bladeUpperArm;
    public Transform bladeLowerArm;
    public Transform bladeHead;
    public Transform blade;

    public float speed = 0.25f;
    private bool on = false;

    void Start() {
    }

    void LateUpdate() {
        if (on) {
            blade.Rotate(Vector3.forward * 360 * 10000 * Time.deltaTime);
        }
    }

    public override IEnumerator Call(string method, string[] arguments, bool block) {
        switch (method) {
            case "MOVE_TO":
                // arguments: [TARGET Y] [TARGET Z]

                Vector3 target = referenceOrigin.TransformPoint(referenceOrigin.InverseTransformPoint(mount.position).x,
                                                                float.Parse(arguments[0]),
                                                                float.Parse(arguments[1]));

                if (block) {
                    yield return StartCoroutine(MoveTo(target));
                } else {
                    StartCoroutine(MoveTo(target));
                }

                break;
            case "ROTATE":
                // arguments: [TARGET_PART] [DEGREES] <opt.: SECONDS>

                float targetDegrees = float.Parse(arguments[1]);
                float degreesPerSecond = (arguments.Length >= 3) ? Mathf.Abs(targetDegrees) / float.Parse(arguments[2]) : 30.0f;
                Transform targetTransform = null;
                Vector3 targetAxis = Vector3.zero;
                switch (arguments[0]) {
                    case "BLADE_UPPER_ARM":
                        targetTransform = bladeUpperArm;
                        targetAxis = Vector3.up;
                        break;
                    case "BLADE_LOWER_ARM":
                        targetTransform = bladeLowerArm;
                        targetAxis = Vector3.forward;
                        break;
                    case "BLADE_HEAD":
                        targetTransform = bladeHead;
                        targetAxis = Vector3.up;
                        break;
                }

                if (block) {
                    yield return StartCoroutine(Rotate(targetTransform, targetAxis, targetDegrees, degreesPerSecond));
                } else {
                    StartCoroutine(Rotate(targetTransform, targetAxis, targetDegrees, degreesPerSecond));
                }

                break;
            case "ON":
                on = true;
                break;
            case "OFF":
                on = false;
                break;
            default:
                yield return null;
                break;
        }
    }

    IEnumerator MoveTo(Vector3 target) {
        float threshold = referenceOrigin.TransformVector(new Vector3(0.01f, 0, 0)).magnitude;
        float adjustedSpeed = referenceOrigin.TransformVector(new Vector3(speed, 0, 0)).magnitude;
        Vector3 diff = (target - mount.position);
        while (diff.magnitude > threshold) {
            wagon.position += Vector3.up * Mathf.Clamp(diff.y, -adjustedSpeed, adjustedSpeed) * Time.deltaTime;
            mount.position += Vector3.forward * Mathf.Clamp(diff.z, -adjustedSpeed, adjustedSpeed) * Time.deltaTime;

            yield return new WaitForFixedUpdate();
            diff = (target - mount.position);
        }
    }

    IEnumerator Rotate(Transform targetTransform, Vector3 axis, float degrees, float degreesPerSecond) {
        float d = 0.0f, inc;
        while (d < Mathf.Abs(degrees)) {
            inc = degreesPerSecond * Time.deltaTime;
            d = Mathf.Clamp(d + inc, 0, Mathf.Abs(degrees));
            targetTransform.Rotate(Mathf.Sign(degrees) * axis * inc);
            yield return new WaitForFixedUpdate();
        }
    }

}
