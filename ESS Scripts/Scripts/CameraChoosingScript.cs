using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraChoosingScript : MonoBehaviour {

	public CameraScript startingCamera;
    private CameraScript[] cameras;
	private int active;
	private float rotSpeed = 15f;

	void Start() {
        cameras = GetComponentsInChildren<CameraScript>();
		int index = 0;
		for (; index < cameras.Length; index++) {
			if (startingCamera == cameras[index])
				break;
		}
		ChangeTo(index);
	}
	
	void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.Period))
        {
            ChangeTo(active + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            ChangeTo(active - 1);
        }
		ControlCamera ();
	}

    void ChangeTo(int ix)
    {
		int d = ix > active ? 1 : -1;
        if (ix < 0)
        {
            ix = cameras.Length - 1;
        }
        else if (ix >= cameras.Length)
        {
            ix = 0;
        }
		if (cameras[ix].turnedOn) {
	        transform.parent = cameras[ix].cameraPivot;
	        transform.localRotation = Quaternion.identity;
	        transform.localPosition = Vector3.zero;
	        active = ix;
		} else {
			ChangeTo(ix + d);
		}
    }

	void ControlCamera() {
		if (Input.GetKey("a"))
		{
			transform.Rotate(Vector3.up, -rotSpeed * Time.deltaTime);
		}
		else if (Input.GetKey("d"))
		{
			transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
		}
		if (Input.GetKey("w"))
		{
			cameras[active].cameraPivot.Rotate(Vector3.right, -rotSpeed * Time.deltaTime);
		}
		else if (Input.GetKey("s"))
		{
			cameras[active].cameraPivot.Rotate(Vector3.right, rotSpeed * Time.deltaTime);
		}
		if (Input.GetKey("q"))
		{
			transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
		}
		else if (Input.GetKey("e"))
		{
			transform.Rotate(Vector3.forward, -rotSpeed * Time.deltaTime);
		}
	}

}
