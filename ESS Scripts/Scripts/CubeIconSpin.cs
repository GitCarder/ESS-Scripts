using UnityEngine;
using System.Collections;

public class CubeIconSpin : MonoBehaviour {


	void Update () {
        transform.Rotate(Vector3.up, Space.World);
	}
}
