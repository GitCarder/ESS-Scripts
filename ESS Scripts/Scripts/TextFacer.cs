using UnityEngine;
using System.Collections;

public class TextFacer : MonoBehaviour {

    void LateUpdate()
    {
        if (Camera.current != null)
        {
            transform.LookAt(Camera.current.transform, Vector3.up);
            transform.Rotate(0, 180, 0);
        }
    }
}
