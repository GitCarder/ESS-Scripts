using UnityEngine;
using System.Collections;

public class Movable : MonoBehaviour {

    public float drag = 1.0f;
    public float acceleration;
    public float maxSpeed;
    public float maxDistance;
    public Vector3 direction;
    public float startPercentage = 0.5f;

    private Vector3 startPosition;
    private float speed = 0.0f;
    private bool updated = false;

    public float distance
    {
        get
        {
            return (transform.localPosition - startPosition).magnitude;
        }
    }
    public Vector3 offset
    {
        get
        {
            return (transform.localPosition - startPosition);
        }
    }

    void Start() {
        startPosition = transform.localPosition;
        transform.localPosition = transform.localPosition + transform.TransformDirection(direction) * maxDistance * startPercentage;
    }

    void LateUpdate() {
        if (!updated) Move(0);
        updated = false;
    }

    public void Move(float c) {
        speed = drag * Mathf.Clamp(speed + c * acceleration * Time.deltaTime, -maxSpeed, maxSpeed);
        Vector3 d = Vector3.ClampMagnitude(transform.localPosition
                                         + transform.TransformDirection(direction) * speed * Time.deltaTime
                                         - startPosition, maxDistance);
        if (Vector3.Dot(d, transform.TransformDirection(direction)) < 0) {
            d = Vector3.zero;
            speed = 0;
        }
        transform.localPosition = startPosition + d;
        updated = true;
    }

}
