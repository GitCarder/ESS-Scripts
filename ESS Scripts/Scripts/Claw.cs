using UnityEngine;

public class Claw : MonoBehaviour {

    private Grip parent;

    void Start() {
        parent = GetComponentInParent<Grip>();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        parent.OnClawCollisionEnter(this, collision);
    }

    void OnCollisionExit(Collision collision) {
        parent.OnClawCollisionExit(this, collision);
    }

}
