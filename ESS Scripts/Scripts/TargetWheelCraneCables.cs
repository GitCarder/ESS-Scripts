using UnityEngine;

[ExecuteInEditMode]
public class TargetWheelCraneCables : MonoBehaviour {

    public Transform start;
    public Transform end;
    
    Vector3 s = Vector3.one;
    Vector3 start_p, end_p;

	void LateUpdate() {
		if (start == null || end == null)
			return;
        transform.position = Vector3.Lerp(start.position, end.position, 0.5f);
        s.z = (end.localPosition - start.localPosition).magnitude / 2;
        transform.localScale = s;
    }

}
