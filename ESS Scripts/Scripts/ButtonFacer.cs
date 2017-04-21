using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ButtonFacer : MonoBehaviour
{

    void LateUpdate()
    {
        if (Camera.current != null) { 
            transform.LookAt(Camera.current.transform);
            transform.Rotate(0,180,0);
        }
    }

}
