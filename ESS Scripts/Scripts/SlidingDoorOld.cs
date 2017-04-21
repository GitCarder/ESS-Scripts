using UnityEngine;
using System.Collections;

public class SlidingDoorOld : MonoBehaviour {

	public float slidingTime = 4.0f;
	public Transform door;
	public Vector3 openPosition;
	private bool closed = true;
	private float t = float.NegativeInfinity;

	void LateUpdate() {
		if (Input.GetKeyDown("return")
		&& (Time.fixedTime - t) > slidingTime) {
			closed = !closed;
			t = Time.fixedTime;
		}
		door.localPosition = Vector3.Lerp (Vector3.zero,
		                                   openPosition,
		                                   Mathf.SmoothStep(closed ? 1 : 0,
		                                                    closed ? 0 : 1,
		                                                    Mathf.Clamp(Time.fixedTime - t,
		            													0,
		            													slidingTime)
		                 													/ slidingTime));
	}

}
