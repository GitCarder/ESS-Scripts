using UnityEngine;

public class ManipulatorElevator : MonoBehaviour {
    
    public Transform tower;
    public Transform towerStart;
    public Transform towerEnd;
    private Vector3 towerScale = new Vector3(1, 1, 1);
    private float originalDistance;

    void Start() {
        originalDistance = transform.InverseTransformVector(towerEnd.position - towerStart.position).magnitude;
    }

    void LateUpdate() {
        float distance = transform.InverseTransformVector(towerEnd.position - towerStart.position).magnitude;
        towerScale.y = distance / originalDistance;
        tower.localScale = towerScale;
    }

}
