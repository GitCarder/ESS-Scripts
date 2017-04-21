using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ik2D : MonoBehaviour {

	public Transform referenceBase;
    public Transform shoulderReference;
    public Transform shoulder;
    public Transform upperarm_reference;
    public Transform upperarm;
    public Transform underarm;
    public Transform underarm_twist_reference;
    public Transform underarm_twist;
    public Transform hand;
    public Transform wrist;
    public Transform handTwistReference;
    public Transform handTwist;
    public Transform targetReference;
    public Transform target;
    public bool IsEnabled = true;
    private float hand_length_local, upperarm_length_local, lowerarm_length_local;

    private bool elbow_flip_allowed = false;
    public bool elbowFlipAllowed {
        get {
            return elbow_flip_allowed;
        }
    }
    private bool elbow_left = false;

	private AudioSource audio;
	private Vector3 lastHandVector;
	private float speedAtMaxVolume = 1.0f;
	private float maxVolume = 0.1f;

    void Start() {
		upperarm_length_local = transform.InverseTransformVector(upperarm.position - underarm.position).magnitude;
		lowerarm_length_local = transform.InverseTransformVector(underarm.position - hand.position).magnitude;
		hand_length_local = transform.InverseTransformVector(hand.position - targetReference.position).magnitude;
		audio = GetComponent<AudioSource>();
		lastHandVector = hand.position - shoulderReference.position;
    }

    void LateUpdate() {
        if (IsEnabled) {
            CalculateIK();
        }

		Vector3 handVector = referenceBase.InverseTransformVector(hand.position - shoulderReference.position);
		float handSpeed = (handVector - lastHandVector).magnitude / Time.deltaTime;
		lastHandVector = handVector;
		audio.volume = Mathf.Lerp(audio.volume, Mathf.Min(handSpeed, speedAtMaxVolume) / speedAtMaxVolume * maxVolume, 0.75f);
    }
    
    void CalculateIK() {
		float upperarm_length = transform.TransformVector(Vector3.forward * upperarm_length_local).magnitude;
		float lowerarm_length = transform.TransformVector(Vector3.forward * lowerarm_length_local).magnitude;
		float hand_length = transform.TransformVector(Vector3.forward * hand_length_local).magnitude;

        /*** Adjust target (solve third limb) ***/
        Vector3 adjustedTarget = target.position - target.forward * hand_length;
        /****************************************/

        /*** Twist underarm ***/
        Vector3 projectedTwist = ProjectVectorOnPlane(target.forward, underarm_twist_reference.up);
        float a = Vector3.Angle(projectedTwist, underarm_twist_reference.right);
        a *= -Mathf.Sign(Vector3.Dot(underarm_twist_reference.forward, projectedTwist));

        Vector3 tmp = underarm_twist.localEulerAngles;
        tmp.y = a;
        underarm_twist.localEulerAngles = tmp;
        /**********************/
        
        /*** Twist hand ***/
        projectedTwist = ProjectVectorOnPlane(target.up, handTwistReference.forward);
        a = Vector3.Angle(projectedTwist, handTwistReference.up);
        a *= -Mathf.Sign(Vector3.Dot(handTwistReference.right, projectedTwist));
            
        tmp = handTwist.localEulerAngles;
        tmp.y = a;
        handTwist.localEulerAngles = tmp;
        /******************/

        /*** Solve shoulder (global y-axis) ***/
        /*Vector3 offset = Vector3.Dot(shoulder.forward, hand.position - shoulderReference.position) * shoulderReference.forward;
        projectedTwist = ProjectVectorOnPlane(adjustedTarget - shoulderReference.position - offset, shoulderReference.up);
        a = Vector3.Angle(projectedTwist, -shoulderReference.right);
        a *= Mathf.Sign(Vector3.Dot(projectedTwist, shoulder.right));

        tmp = shoulder.localEulerAngles;
        tmp.y = a;
        shoulder.localEulerAngles = tmp;*/

        
        Vector3 projectedHeight_target = ProjectVectorOnPlane(adjustedTarget - shoulderReference.position, shoulderReference.up);
        Vector3 projectedHeight_reference = ProjectVectorOnPlane(hand.position - shoulderReference.position, shoulderReference.up);

        float diff = Vector3.Angle(projectedHeight_reference, projectedHeight_target);
        diff *= Mathf.Sign(Vector3.Dot(shoulderReference.forward, projectedHeight_target - projectedHeight_reference));
        
        tmp = shoulder.localEulerAngles;
        tmp.y += diff;
        shoulder.localEulerAngles = tmp;
        /**************************************/

        /*** Project target T onto upperarm plane (upperarm pivot up as normal) ***/
        Vector3 adjustedTarget_projected = ProjectPointOnPlane(adjustedTarget, upperarm_reference.position, upperarm_reference.up);
        /**************************************************************************/
        
        // Calculate distance to adjusted (projected) target position
        float target_distance = Mathf.Min(Vector3.Distance(upperarm_reference.position, adjustedTarget_projected), upperarm_length + lowerarm_length);
        
        if (target_distance < Mathf.Abs(upperarm_length - lowerarm_length)) {
            /*** Target position is too close to the center ***/
            Debug.LogError("IK is unsolvable.");
        } else {
            /*** If arm is straight, allow the elbow to flip. ***/
            elbow_flip_allowed = Vector3.Distance(upperarm_reference.position, adjustedTarget_projected) >= target_distance;
            /****************************************************/

            /*** Calculate elbow position ***/
            float alpha = Vector3.Angle(upperarm_reference.forward, adjustedTarget_projected - upperarm_reference.position);
            float gamma = Mathf.Acos((Mathf.Pow(target_distance, 2) + Mathf.Pow(upperarm_length, 2) - Mathf.Pow(lowerarm_length, 2)) / (2 * upperarm_length * target_distance)) * Mathf.Rad2Deg;

            bool right = Vector3.Dot(adjustedTarget_projected - upperarm_reference.position, upperarm_reference.right) > 0;
            float upperarm_theta = alpha + (elbow_left ^ right ? 1 : -1) * gamma;

            if (right)
                upperarm_theta *= -1;

            tmp = upperarm.localEulerAngles;
            tmp.y = -upperarm_theta;
            upperarm.localEulerAngles = tmp;
            /********************************/

            /*** Solve underarm ***/
            underarm.LookAt(ProjectPointOnPlane(adjustedTarget, underarm.position, upperarm_reference.up), upperarm_reference.up);
            /**********************/

            /*** Solve hand ***/
            hand.LookAt(hand.position + ProjectVectorOnPlane(target.forward, hand.up), hand.up);
            /******************/
        }

        CalculateConstraints();
    }

    void CalculateConstraints() {
    }

    Vector3 ClampY(Vector3 v, float max) {
        v.y = Mathf.Clamp(v.y % 360, -max, max);
        return v;
    }

    Vector3 ProjectPointOnPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal) {
        return point + planeNormal.normalized * Vector3.Dot(planeNormal.normalized, planePoint - point);
    }

    Vector3 ProjectVectorOnPlane(Vector3 v, Vector3 planeNormal) {
        return v - planeNormal.normalized * Vector3.Dot(planeNormal.normalized, v);
    }

}


