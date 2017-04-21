using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {
    
	void Update () {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
	}

}
