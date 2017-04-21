using UnityEngine;
using System.Collections;

public class FalconTarget : MonoBehaviour, FalconListener {

	private Vector3 originPosition;
	private Quaternion originRotation;

	public void Set(Vector3 position, Quaternion rotation) {
		originPosition = position;
		originRotation = rotation;
		transform.localPosition = originPosition;
		transform.rotation = originRotation;
	}

	public void SetFalconActive(bool this_falcon_active){
	}

	Vector3 lastTipPosition = Vector3.zero;
	public Vector3 FalconTipPosition(Vector3 position) {
		transform.localPosition = originPosition + new Vector3(position.x, position.y, position.z) * 160;

		return Vector3.zero;
	}
	
	public void FalconButtonPressed(int i) {
	}
	
	public void FalconButtonReleased(int i) {
	}
	
	void OnCollisionStay(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
		}
	}
	
}
