using UnityEngine;
using System.Collections;

public class ModelController : MonoBehaviour {
    
    public Transform modelPosition;
    public Transform modelRotation;
    public Transform modelViewReference;

    public Camera3D camera3d;
    
	void LateUpdate() {
        // Transform model appropriately.
        modelPosition.position -= camera3d.transform.position - modelViewReference.position;

        modelRotation.localEulerAngles = new Vector3(camera3d.transform.localEulerAngles.x, -camera3d.transform.localEulerAngles.y, -camera3d.transform.localEulerAngles.z);
        //modelRotation.rotation = Quaternion.Inverse(camera3d.transform.rotation) * Quaternion.LookRotation(modelViewReference.forward);


        transform.localScale = camera3d.transform.localScale;
    }

	void Update() {
	}

    public void FalconButtonPressed(int i) {
    }

    public void FalconButtonReleased(int i) {
    }

    public void FalconTipPosition(Vector3 position) {
    }

	public void SetFalconActive(bool this_falcon_active) {
	}

}
