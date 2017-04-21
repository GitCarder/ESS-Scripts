using UnityEngine;
using System.Collections;

public class Grip : MonoBehaviour {

    public Transform claw1;
    public Transform claw2;
    private Vector3 claw1_origin;
    private Vector3 claw2_origin;
    public Vector3 claw1_closed;
    public Vector3 claw2_closed;
    private Collision claw1_collision;
    private Collision claw2_collision;
    private float speed = 0.2f;
	private float current_t = 0;

    void Start() {
        claw1_origin = claw1.localPosition;
        claw2_origin = claw2.localPosition;
    }

    void LateUpdate() {
    }

	public void SetT(float t) {
		claw1.localPosition = Vector3.Lerp(claw1_origin, claw1_closed, t);
		claw2.localPosition = Vector3.Lerp(claw2_origin, claw2_closed, t);
		current_t = t;
	}

	public IEnumerator GripT(float target_t, float seconds) {
		for (float s = 0; s <= 1; s = Mathf.Min(1, s + Time.deltaTime / seconds)) {
			SetT(current_t + (target_t - current_t) * s);
			if (s == 1)
				break;
			yield return new WaitForFixedUpdate();
		}
	}

    public void OnClawCollisionEnter(Claw claw, Collision collision) {
        if (claw.transform == claw1) {
            claw1_collision = collision;
        } else if (claw.transform == claw2) {
            claw2_collision = collision;
        }
    }

    public void OnClawCollisionExit(Claw claw, Collision collision) {
        if (claw.transform == claw1) {
            claw1_collision = null;
        } else if (claw.transform == claw2) {
            claw2_collision = null;
        }
    }

}
