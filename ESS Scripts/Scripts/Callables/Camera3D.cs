using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Camera3D : Callable, FalconListener {

  	public Window3D window;
  	private bool isZooming = false;
	private bool isFollowing = false;
	private float maxSpeed = 0.5f;

	private bool falconZoom = false;
	private bool falconMove = false;
	private bool falconRotate = false;
	private Vector3 pos; 
	private Quaternion startRotation;

	void Start(){
		startRotation = transform.localRotation;
		Debug.Log (startRotation);
	}

	void LateUpdate() {
    if (!isZooming)
      UpdateModel();
		if (Input.GetKeyDown (KeyCode.F12))
			StartCoroutine (ZoomTo (GameObject.Find ("COMPLETE_NOZOOM").transform, 5));
  }

  void UpdateModel() {
      transform.parent.localScale *= window.transform.lossyScale.magnitude / transform.lossyScale.magnitude;
      transform.parent.position += window.transform.position - transform.position;
      transform.parent.rotation *= window.transform.rotation * Quaternion.Inverse(transform.rotation);
  }

	public override IEnumerator Call(string method, string[] arguments, bool block) {
		switch (method) {
		case "ZOOM":
      isZooming = true;
			isFollowing = false;
			// arguments : [TARGET_ZOOM] <opt. seconds>
			Transform target = GameObject.Find (arguments[0]).transform;
			float seconds = arguments.Length > 1 ? float.Parse(arguments[1]) : 3;
			if (block) {
				yield return StartCoroutine(ZoomTo(target, seconds));
			} else {
				StartCoroutine(ZoomTo(target, seconds));
			}
			break;
		case "FOLLOW":
			// arguments : [TARGET_ZOOM]
			StartCoroutine(Follow(GameObject.Find (arguments[0]).transform));
			break;
		case "UNFOLLOW":
			isFollowing = false;
			break;
		}
	}

	IEnumerator Follow(Transform target) {
		isFollowing = true;
		while (isFollowing) {
			transform.position = Vector3.MoveTowards (transform.position, target.position, transform.TransformVector(Vector3.forward * maxSpeed).magnitude * Time.deltaTime);
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator ZoomTo(Transform reference, float seconds) {
        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = transform.parent.InverseTransformPoint(reference.position);

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = reference.localScale;

		Quaternion startRotation = transform.localRotation;
		Quaternion targetRotation = transform.localRotation * Quaternion.Inverse(transform.rotation) * reference.rotation;

		for (float t = 0; !isFollowing && t <= 1.0f; t = Mathf.Min(1, t + Time.deltaTime / seconds)) {
			float s = Mathf.SmoothStep(0, 1, t);

			transform.localPosition = Vector3.Lerp(startPosition, targetPosition, s);
			transform.localScale = Vector3.Lerp(startScale, targetScale, s);
			transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, s);
      UpdateModel();

            if (t == 1.0f)
                break;
            yield return new WaitForFixedUpdate();
        }
        isZooming = false;
    }
	
	public void SetFalconActive(bool this_falcon_active){
	}

	Vector3 localScale;
	Vector3 localPos;
	Vector3 localRotation;
	float zoomSpeed = 100;
	float moveSpeed = 100;
	float rotationSpeed = 1000;
	float startZ;
	Vector3 startPos;
	float startX;
	float localZ;
	public Vector3 FalconTipPosition(Vector3 position) {
		pos = position;
		if (falconZoom) {
			transform.localScale = localScale + Vector3.one * (position.z - startZ) * zoomSpeed;
		} else if (falconMove) {
			transform.localPosition = localPos;
			Vector3 diff = (position - startPos) * moveSpeed;
			transform.position -= diff.x * transform.right + diff.y * transform.up + diff.z * transform.forward;
		} else if (falconRotate) {
			transform.localEulerAngles = localRotation + new Vector3(0,1,0) * (position.x - startX) * rotationSpeed;
		}
		return Vector3.zero;
	}
	
	public void FalconButtonPressed(int i) {
		switch (i) {
			case 0:
				break;
			case 1:
				falconRotate = true;
				localRotation = transform.localEulerAngles;
				startX = pos.x;
				break;
			case 2:
				falconZoom = true;
				localScale = transform.localScale;
				startZ = pos.z;				
				break;
			case 3:
				falconMove = true;
				localPos = transform.localPosition;
				startPos = pos;
				break;
		}
	}
	
	public void FalconButtonReleased(int i) {
		switch (i) {
			case 0:
				break;
			case 1:
				falconRotate = false;
				break;
			case 2:				
				falconZoom = false;
				localScale = transform.localScale;
				break;
			case 3:
				falconMove = false;
				break;
		}
	}


}
