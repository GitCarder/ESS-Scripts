using UnityEngine;
using System.Collections;

public class CamMovement : MonoBehaviour {

    private float speed = 0.5f;
    private float initYrot;

    void Start()
    {
        initYrot = transform.rotation.eulerAngles.y;
    }

	void Update () {
        transform.rotation = Quaternion.Euler(0, initYrot - Mathf.Sin(Time.realtimeSinceStartup * speed) * 70, 0);
    }
}
