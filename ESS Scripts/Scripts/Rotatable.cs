using UnityEngine;
using System.Collections;

public class Rotatable : MonoBehaviour {

    public Vector3 pivot = Vector3.up;
    public float drag = 0.95f;
    public float acceleration = 60.0f;
    public float maxSpeed = 60.0f;
    public float maxRotation = 360;
    public float angle = 0.0f;
    private float speed = 0.0f;
    private Quaternion startRotation;
    private bool updated = false;

    void Start() {
        startRotation = transform.localRotation;
    }

    void LateUpdate() {
        if (!updated) Rotate(0);
        updated = false;
    }

    public void Rotate(float c) {
        speed = drag * Mathf.Clamp(speed + c * acceleration * Time.deltaTime, -maxSpeed, maxSpeed);
        if (maxRotation >= 360) {
            angle = (angle + speed) % 360.0f;
        } else {
            angle = Mathf.Clamp(angle + speed, 0, maxRotation);
        }
        transform.localRotation = startRotation * Quaternion.AngleAxis(angle, pivot);
        updated = true;
    }

}
