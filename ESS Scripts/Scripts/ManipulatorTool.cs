using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
public class ManipulatorTool : MonoBehaviour {

    public Transform gripReference;
    public Transform leftClawReference;
    public Transform rightClawReference;	   

    void OnTriggerEnter(Collider other) {
        Manipulator manipulator = other.GetComponent<Manipulator>();
        if (manipulator != null) {
            manipulator.ToolInRange(this, true);
        }
    }

    void OnTriggerExit(Collider other) {
        Manipulator manipulator = other.GetComponent<Manipulator>();
        if (manipulator != null) {
            manipulator.ToolInRange(this, false);
        }
    }

}
