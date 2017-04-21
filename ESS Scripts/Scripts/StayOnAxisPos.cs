using UnityEngine;
using System.Collections;
using Leap;

[ExecuteInEditMode]
public class StayOnAxisPos : MonoBehaviour {

	public Transform obj;
	public LineRenderer xBeam;
	public LineRenderer yBeam;
	public LineRenderer zBeam;
	private bool active = true;

	void LateUpdate () {
		if (!active) {
			return;
		}
		Beam (xBeam, transform.right);
		Beam (yBeam, transform.up);
		Beam (zBeam, transform.forward);
	}

	void Beam(LineRenderer line, Vector3 direction) {
		line.SetPosition(0, TryRaycast(direction));
		line.SetPosition(1, TryRaycast(-direction));
	}

	Vector3 TryRaycast(Vector3 direction) {
		RaycastHit hit;
		foreach (Collider collider in GetComponents<Collider>()) {
			if (collider.Raycast (new Ray(obj.position, direction), out hit, 100)) {
				return hit.point;
			}
		}
		return obj.position;
	}

}
