using UnityEngine;
using System.Collections;

public class TouchPadAnimation : MonoBehaviour {

	public Transform Button;

	private ButtonDemoToggle button;
	private Vector3 gone_pos;
	private Vector3 present_pos;
	private float speed = 1.0f;

	void Start () {
		button = Button.GetComponentInChildren<ButtonDemoToggle> ();
		button.ToggleState = false;
		button.ButtonTurnsOff ();
		present_pos = transform.localPosition;
		gone_pos = present_pos;
		gone_pos.y = present_pos.y + 0.9f;
		transform.localPosition = gone_pos;
	}

	void Update () {
		if (button.ToggleState == true) {
			Debug.Log("TouchActive");
			transform.localPosition = Vector3.Lerp(gone_pos, present_pos, speed*Time.deltaTime);
		}
	}
}
