using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Manipulator : Callable, FalconListener {

    public Transform referenceBase;
    public Transform referenceOrigin;

    public Transform wagon;
    public Transform armBase;

    public Transform torso;
    public Transform leftShoulder;
    public Transform leftUpperArm;
    public Transform leftElbow;
    public Transform leftLowerArm;
    public Transform leftWrist;
    public Transform leftHand;
    public Transform rightShoulder;
    public Transform rightUpperArm;
    public Transform rightElbow;
    public Transform rightLowerArm;
    public Transform rightWrist;
    public Transform rightHand;

    public Vector3 torsoAxis;
    public Vector3 leftShoulderAxis;
    public Vector3 leftUpperArmAxis;
    public Vector3 leftElbowAxis;
    public Vector3 leftLowerArmAxis;
    public Vector3 leftWristAxis;
    public Vector3 leftHandAxis;
    public Vector3 rightShoulderAxis;
    public Vector3 rightUpperArmAxis;
    public Vector3 rightElbowAxis;
    public Vector3 rightLowerArmAxis;
    public Vector3 rightWristAxis;
    public Vector3 rightHandAxis;

    private Vector3 torsoStartEuler;
    private Vector3 leftShoulderStartEuler;
    private Vector3 leftUpperArmStartEuler;
    private Vector3 leftElbowStartEuler;
    private Vector3 leftLowerArmStartEuler;
    private Vector3 leftWristStartEuler;
    private Vector3 leftHandStartEuler;
    private Vector3 rightShoulderStartEuler;
    private Vector3 rightUpperArmStartEuler;
    private Vector3 rightElbowStartEuler;
    private Vector3 rightLowerArmStartEuler;
    private Vector3 rightWristStartEuler;
    private Vector3 rightHandStartEuler;

    public Transform targetReference;
    public Transform targetReference_arm1;
    public Transform targetReference_arm2;
    public Transform ik_target_arm1;
    public Transform ik_target_arm2;
    public ik2D ik2D_arm1;
    public ik2D ik2D_arm2;
    private Transform attachedTransform_arm1;
    private Transform attachedTransform_arm2;

    private bool followLeft = false;
    private bool followRight = false;

    private List<ManipulatorTool> toolsInRange = new List<ManipulatorTool>();

    public StayOnAxisPos stayOnAxisPos;
	public Transform reference;

	//Falcon
	public Transform arrow;
	public float maxSpeed = 1.0f;
	public float rotationSpeed = 1.0f;
	private Vector3 direction = Vector3.zero;
	private bool falcon_translate = false;
	private bool falcon_rotate_left = false;
	private Vector3 falconVelocity = Vector3.zero;
	private Vector3 falconAcceleration = new Vector3(1, 1, 1);
	private bool falcon_rotate_right = false;

    Transform tmpTransform;

	// Sound
	public AudioSource audiosource;
	private Vector3 lastPos;

	public AudioClip nearingSound;
	public AudioSource nearingSource;
	private float sinceLastNearingSound = 0;

    void Start() {
        torsoStartEuler = torso.localEulerAngles;
        leftShoulderStartEuler = leftShoulder.localEulerAngles;
        leftUpperArmStartEuler = leftUpperArm.localEulerAngles;
        leftElbowStartEuler = leftElbow.localEulerAngles;
        leftLowerArmStartEuler = leftLowerArm.localEulerAngles;
        leftWristStartEuler = leftWrist.localEulerAngles;
        leftHandStartEuler = leftHand.localEulerAngles;
        rightShoulderStartEuler = rightShoulder.localEulerAngles;
        rightUpperArmStartEuler = rightUpperArm.localEulerAngles;
        rightElbowStartEuler = rightElbow.localEulerAngles;
        rightLowerArmStartEuler = rightLowerArm.localEulerAngles;
        rightWristStartEuler = rightWrist.localEulerAngles;
        rightHandStartEuler = rightHand.localEulerAngles;

        tmpTransform = new GameObject("Temporary transform").transform;
        tmpTransform.parent = referenceBase;

		arrow.gameObject.SetActive(false);
		reference.gameObject.SetActive (false);
    }

	void LateUpdate() {
		falconVelocity *= 0.94f;
		if (falcon_translate) {
			float max = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));

			if (Mathf.Abs(direction.x) == max) {
				falconVelocity.x += Mathf.Sign(direction.x) * falconAcceleration.x * Time.deltaTime;
			} else if (Mathf.Abs(direction.z) == max) {
				falconVelocity.y += Mathf.Sign(direction.z) * falconAcceleration.y * Time.deltaTime;
			} else if (Mathf.Abs(direction.y) == max) {
				falconVelocity.z -= Mathf.Sign(direction.y) * falconAcceleration.z * Time.deltaTime;
			}
			falconVelocity = Vector3.ClampMagnitude(falconVelocity, maxSpeed);

		} else if(falcon_rotate_left) {
			torso.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * rotationSpeed);
		} else if(falcon_rotate_right) {
			torso.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotationSpeed);
		}

		transform.position += referenceBase.TransformVector(Vector3.right * falconVelocity.x * Time.deltaTime);
		armBase.position += referenceBase.TransformVector(Vector3.up * falconVelocity.y * Time.deltaTime);
		wagon.position += referenceBase.TransformVector(Vector3.forward * falconVelocity.z * Time.deltaTime);

        // Restrict movement
        Vector3 l = transform.localPosition;
        l.x = Mathf.Clamp(l.x, -23.5f, -15.5f);
        transform.localPosition = l;

        l = armBase.transform.localPosition;
        l.y = Mathf.Clamp(l.y, 26.5f, 70);
        armBase.transform.localPosition = l;

        l = wagon.transform.localPosition;
        l.y = Mathf.Clamp(l.y, -45f, 45f);
        wagon.transform.localPosition = l;
        
		// Sounds
		if (Time.fixedTime > 1) {
			Vector3 newPos = referenceBase.InverseTransformPoint (targetReference.position);
			float currSpeed = ((newPos - lastPos).magnitude / Time.deltaTime) / maxSpeed;
			audiosource.volume = Mathf.Lerp (audiosource.volume, Mathf.Min (1, currSpeed), 0.5f);
			audiosource.pitch = 0.75f + audiosource.volume / 4;
			lastPos = newPos;
		}
		sinceLastNearingSound += Time.deltaTime;
    }

    public override IEnumerator Call(string method, string[] arguments, bool block) {
        switch (method)
        {
            case "SET_ROTATION":

                Transform targetTransform = null;
                Vector3 startEuler = Vector3.zero;
                Vector3 targetAxis = Vector3.zero;
                bool left = false;
                float targetDegrees = 0;

                switch (arguments[0])
                {
                    case "TORSO":
                        targetTransform = torso;
                        targetAxis = torsoAxis;
                        startEuler = torsoStartEuler;
                        targetDegrees = float.Parse(arguments[1]);
                        break;
                    case "LEFT":
                        left = true;
                        break;
                    case "RIGHT":
                        left = false;
                        break;
                }

                if (targetTransform == null)
                {
                    switch (arguments[1])
                    {
                        case "SHOULDER":
                            targetTransform = left ? leftShoulder : rightShoulder;
                            targetAxis = left ? leftShoulderAxis : rightShoulderAxis;
                            startEuler = left ? leftShoulderStartEuler : rightShoulderStartEuler;
                            break;
                        case "UPPER_ARM":
                            targetTransform = left ? leftUpperArm : rightUpperArm;
                            targetAxis = left ? leftUpperArmAxis : rightUpperArmAxis;
                            startEuler = left ? leftUpperArmStartEuler : leftUpperArmStartEuler;
                            break;
                        case "ELBOW":
                            targetTransform = left ? leftElbow : rightElbow;
                            targetAxis = left ? leftElbowAxis : rightElbowAxis;
                            startEuler = left ? leftElbowStartEuler : leftElbowStartEuler;
                            break;
                        case "LOWER_ARM":
                            targetTransform = left ? leftLowerArm : rightLowerArm;
                            targetAxis = left ? leftLowerArmAxis : rightLowerArmAxis;
                            startEuler = left ? leftLowerArmStartEuler : leftLowerArmStartEuler;
                            break;
                        case "WRIST":
                            targetTransform = left ? leftWrist : rightWrist;
                            targetAxis = left ? leftWristAxis : rightWristAxis;
                            startEuler = left ? leftWristStartEuler : leftWristStartEuler;
                            break;
                        case "HAND":
                            targetTransform = left ? leftHand : rightHand;
                            targetAxis = left ? leftHandAxis : rightHandAxis;
                            startEuler = left ? leftHandStartEuler : leftHandStartEuler;
                            break;
                    }
                    targetDegrees = float.Parse(arguments[2]);

                    Unfollow(arguments[0]);
                }

                targetTransform.localEulerAngles = startEuler;
                targetTransform.Rotate(targetAxis * targetDegrees);

                break;
            case "SET_POSITION":
                Vector3 target = referenceOrigin.TransformPoint(float.Parse(arguments[0]),
                                                        float.Parse(arguments[1]),
                                                        float.Parse(arguments[2]));
                transform.position = target.x * Vector3.right;
                armBase.transform.position = target.y * Vector3.up;
                wagon.transform.position = target.z * Vector3.forward;
                break;
            case "MOVE":
                // arguments : [DIFF_X DIFF_Y DIFF_Z] <opt. seconds>
                // Moves manipulator to relative position

                Vector3 current = referenceBase.InverseTransformPoint(targetReference.position) - referenceOrigin.localPosition;
                Vector3 diff = new Vector3(-float.Parse(arguments[0]), float.Parse(arguments[1]), float.Parse(arguments[2]));
                Vector3 targetPosition = new Vector3(current.x + diff.x, current.y + diff.y, current.z + diff.z);

				float seconds = (arguments.Length == 4) ? float.Parse(arguments[1]) : (diff - current).magnitude / maxSpeed;

                if (block) {
                    yield return StartCoroutine(MoveTo(targetPosition, seconds));
                } else {
                    StartCoroutine(MoveTo(targetPosition, seconds));
                }

                break;
            case "MOVE_TO":
                // arguments : [TARGET_TRANSFORM] | [TARGET_X TARGET_Y TARGET_Z] <opt. seconds>
                // Moves manipulator to absolute position

                seconds = -1;

                if (arguments.Length == 1 || arguments.Length == 2) {
                    targetPosition = referenceBase.InverseTransformPoint(GameObject.Find(arguments[0]).transform.position) - referenceOrigin.localPosition;
                    if (arguments.Length == 2)
                        seconds = float.Parse(arguments[1]);
                } else {
                    current = referenceBase.InverseTransformPoint(targetReference.position) - referenceOrigin.localPosition;
                    float x = arguments[0].Equals("*") ? current.x : float.Parse(arguments[0]);
                    float y = arguments[1].Equals("*") ? current.y : float.Parse(arguments[1]);
                    float z = arguments[2].Equals("*") ? current.z : float.Parse(arguments[2]);
                    targetPosition = new Vector3(x, y, z);
                    if (arguments.Length == 4)
                        seconds = float.Parse(arguments[3]);
                }

                if (seconds == -1)
					seconds = referenceOrigin.InverseTransformVector(targetPosition - targetReference.position).magnitude / maxSpeed;

			if (block) {
                    yield return StartCoroutine(MoveTo(targetPosition, seconds));
                } else {
                    StartCoroutine(MoveTo(targetPosition, seconds));
                }

                break;
            case "FOLLOW":
                // Follows transform using inverse kinematics, cancels previous follow.
                // Will try to reach point/transform, and not stop until unfollow is called.
                // arguments: [SIDE]

                StartCoroutine(Follow(arguments[0], GameObject.Find(arguments[1]).transform));

                break;
            case "UNFOLLOW":
                // Cancels follow
                // arguments: [SIDE]

                Unfollow(arguments[0]);

                break;
            case "REACH_FOR":
                // Inverse kinematics (shoulder, upper arm, lower arm)
                // Will try to reach point/transform, and then stop.
                // Calling this method will cancel follow

                // arguments: [SIDE | TARGET_PART] [[POS_X POS_Y POS_Z] | [TARGET_TRANSFORM]] <opt.: SECONDS>

                Unfollow(arguments[0]);

                seconds = 2.0f;
                if (arguments.Length == 2 || arguments.Length == 3) {
                    targetTransform = GameObject.Find(arguments[1]).transform;
                    if (arguments.Length == 3)
                        seconds = float.Parse(arguments[2]);
                } else {
                    float x = arguments[1].Equals("*") ? 0 : float.Parse(arguments[1]);
                    float y = arguments[2].Equals("*") ? 0 : float.Parse(arguments[2]);
                    float z = arguments[3].Equals("*") ? 0 : float.Parse(arguments[3]);
                    targetTransform = tmpTransform;
                    targetTransform.localPosition = referenceBase.InverseTransformPoint(targetReference.position) + new Vector3(x, y, z);
                    if (arguments.Length == 5)
                        seconds = float.Parse(arguments[4]);
                }
                switch (arguments[0]) {
                    case "LEFT":
                        if (block) {
                            yield return StartCoroutine(ReachFor(ik_target_arm1, ik2D_arm1, targetReference_arm1, targetTransform, seconds));
                        } else {
                            StartCoroutine(ReachFor(ik_target_arm1, ik2D_arm1, targetReference_arm1, targetTransform, seconds));
                        }
                        break;
                    case "RIGHT":
                        if (block) {
                            yield return StartCoroutine(ReachFor(ik_target_arm2, ik2D_arm2, targetReference_arm2, targetTransform, seconds));
                        } else {
                            StartCoroutine(ReachFor(ik_target_arm2, ik2D_arm2, targetReference_arm2, targetTransform, seconds));
                        }
                        break;
                }
                break;
            case "ROTATE":
                // arguments: [SIDE | TARGET_PART] [DEGREES] <opt.: SECONDS>
                targetTransform = null;
                targetAxis = Vector3.zero;
                left = false;
                targetDegrees = 0;
                seconds = -1;

                switch (arguments[0]) {
                    case "TORSO":
                        targetTransform = torso;
                        targetAxis = torsoAxis;
                        targetDegrees = float.Parse(arguments[1]);
                        if (arguments.Length >= 3)
                            seconds = float.Parse(arguments[2]);
                        break;
                    case "LEFT":
                        left = true;
                        break;
                    case "RIGHT":
                        left = false;
                        break;
                }
                if (targetTransform == null) {
                    switch (arguments[1]) {
                        case "SHOULDER":
                            targetTransform = left ? leftShoulder : rightShoulder;
                            targetAxis = left ? leftShoulderAxis : rightShoulderAxis;
                            break;
                        case "UPPER_ARM":
                            targetTransform = left ? leftUpperArm : rightUpperArm;
                            targetAxis = left ? leftUpperArmAxis : rightUpperArmAxis;
                            break;
                        case "ELBOW":
                            targetTransform = left ? leftElbow : rightElbow;
                            targetAxis = left ? leftElbowAxis : rightElbowAxis;
                            break;
                        case "LOWER_ARM":
                            targetTransform = left ? leftLowerArm : rightLowerArm;
                            targetAxis = left ? leftLowerArmAxis : rightLowerArmAxis;
                            break;
                        case "WRIST":
                            targetTransform = left ? leftWrist : rightWrist;
                            targetAxis = left ? leftWristAxis : rightWristAxis;
                            break;
                        case "HAND":
                            targetTransform = left ? leftHand : rightHand;
                            targetAxis = left ? leftHandAxis : rightHandAxis;
                            break;
                    }
                    targetDegrees = float.Parse(arguments[2]);

                    if (arguments.Length >= 4)
                        seconds = float.Parse(arguments[3]);

                    Unfollow(arguments[0]);
                }

                if (seconds == -1)
                    seconds = Mathf.Abs(targetDegrees) / 30;

                if (block) {
                    yield return StartCoroutine(Rotate(targetTransform, targetAxis, targetDegrees, seconds));
                } else {
                    StartCoroutine(Rotate(targetTransform, targetAxis, targetDegrees, seconds));
                }

                break;
            case "SET_GRIP":
                Grip grip = null;
                switch (arguments[0]) {
                    case "LEFT":
                        grip = leftHand.GetComponent<Grip>();
                        break;
                    case "RIGHT":
                        grip = rightHand.GetComponent<Grip>();
                        break;
                }

                grip.SetT(float.Parse(arguments[1]));
                break;
            case "GRIP":
                // Grips the attached object, or t=[0,1]
                // - where t=0 is completely open and t=1 is completely closed
                // arguments: [SIDE] <opt. T> <opt.: SECONDS>

                grip = null;
                float t = -1;
                switch (arguments[0]) {
                    case "LEFT":
                        grip = leftHand.GetComponent<Grip>();
				GripTarget gripTarget = leftHand.GetComponentInChildren<GripTarget>();
                        if (gripTarget != null)
                            t = gripTarget.target_T;
                        break;
                    case "RIGHT":
                        grip = rightHand.GetComponent<Grip>();
                        gripTarget = rightHand.GetComponentInChildren<GripTarget>();
                        if (gripTarget != null)
                            t = gripTarget.target_T;
                        break;
                }
                if (t == -1) {
                    t = float.Parse(arguments[1]);
                    seconds = arguments.Length >= 3 ? float.Parse(arguments[2]) : 2.0f;
                } else {
                    seconds = arguments.Length >= 2 ? float.Parse(arguments[1]) : 2.0f;
                }

                if (block) {
					yield return StartCoroutine(grip.GripT(t, seconds));
                } else {
					StartCoroutine(grip.GripT(t, seconds));
                }

                break;
            case "ATTACH":
                Deattach(arguments[0]);
                switch (arguments[0]) {
                    case "LEFT":
                        attachedTransform_arm1 = GameObject.Find(arguments[1]).transform;
                        attachedTransform_arm1.parent = targetReference_arm1;
                        break;
                    case "RIGHT":
                        attachedTransform_arm2 = GameObject.Find(arguments[1]).transform;
                        attachedTransform_arm2.parent = targetReference_arm2;
                        break;
                }
                break;
            case "DEATTACH":
                Deattach(arguments[0]);
                break;
			case "WAIT_UNTIL_REACHED":
				// Waits until given hand has reached target, or unfollow has been called, or if any other movement has been requested.
				// Cancels follow.
				yield return StartCoroutine(WaitUntilReached(arguments[0], GameObject.Find(arguments[1]).transform, 0.03f));
				break;
			case "WAIT_UNTIL_MOVED_TO":
				// Waits until manipulator has reached target
			yield return StartCoroutine(WaitUntilMovedTo(GameObject.Find(arguments[0]).transform, 0.50f));
				break;
        }
    }

    IEnumerator Rotate(Transform targetTransform, Vector3 axis, float degrees, float seconds) {
        Quaternion startRotation = targetTransform.localRotation;
        for (float t = 0.0f; t <= 1.0f; t = Mathf.Min(1, t + Time.deltaTime / seconds)) {
            targetTransform.localRotation = startRotation * Quaternion.AngleAxis(Mathf.SmoothStep(0, 1, t) * degrees, axis);
            if (t == 1.0f) {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator MoveTo(Vector3 targetPosition, float seconds) {
        Vector3 startPosition = referenceBase.InverseTransformPoint(targetReference.position) - referenceOrigin.localPosition;

        float offset = (referenceBase.InverseTransformPoint(transform.position) - startPosition).x;
        float offset_armBase = (referenceBase.InverseTransformPoint(armBase.transform.position) - startPosition).y;
        float offset_wagon = (referenceBase.InverseTransformPoint(wagon.transform.position) - startPosition).z;

        Vector3 d, tmp;

        for (float t = 0.0f; t <= 1.0f; t = Mathf.Min(1, t + Time.deltaTime / seconds)) {
            d = Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, t));

            tmp = referenceBase.InverseTransformPoint(transform.position);
            tmp.x = d.x + offset;
            transform.position = referenceBase.TransformPoint(tmp);

            tmp = referenceBase.InverseTransformPoint(armBase.transform.position);
            tmp.y = d.y + offset_armBase;
            armBase.transform.position = referenceBase.TransformPoint(tmp);

            tmp = referenceBase.InverseTransformPoint(wagon.transform.position);
            tmp.z = d.z + offset_wagon;
            wagon.transform.position = referenceBase.TransformPoint(tmp);

            if (t == 1.0f) {
                // just to make sure that t=1 is included
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void Deattach(string arm) {
        switch (arm) {
            case "LEFT":
                if (attachedTransform_arm1 != null) {
                    attachedTransform_arm1.parent = referenceBase;
                    attachedTransform_arm1 = null;
                }
                break;
            case "RIGHT":
                if (attachedTransform_arm2 != null) {
                    attachedTransform_arm2.parent = referenceBase;
                    attachedTransform_arm2 = null;
                }
                break;
        }
    }

    public void ToolInRange(ManipulatorTool tool, bool inRange) {
        if (inRange) {
            if (!toolsInRange.Contains(tool))
                toolsInRange.Add(tool);
        } else {
            toolsInRange.Remove(tool);
        }
    }

    IEnumerator ReachFor(Transform ik_target, ik2D ik2D_arm, Transform targetReference, Transform targetTransform, float seconds) {
        ik2D_arm.IsEnabled = true;

        Vector3 startPosition = referenceBase.InverseTransformPoint(targetReference.position);
        Quaternion startRotation = targetReference.rotation;

        for (float t = 0.0f; t <= 1.0f; t = Mathf.Min(1, t + Time.deltaTime / seconds))
        {
            ik_target.position = Vector3.Lerp(referenceBase.TransformPoint(startPosition),
                                              targetTransform.position,
                                              Mathf.SmoothStep(0, 1, t));
            ik_target.rotation = Quaternion.Lerp(startRotation, targetTransform.rotation, Mathf.SmoothStep(0, 1, t));
            if (t == 1.0f) {
                // just to make sure that t=1 is included
                break;
            }
            yield return new WaitForFixedUpdate();
        }

        ik2D_arm.IsEnabled = false;
    }

    IEnumerator Follow(string side, Transform targetTransform) {
        Unfollow(side);

        Transform ik_target;
        ik2D ik2D_arm;
        Transform targetReference;
        bool left;

        switch (side) {
            case "LEFT":
                ik_target = ik_target_arm1;
                ik2D_arm = ik2D_arm1;
                targetReference = targetReference_arm1;
                followLeft = true;
                left = true;
                break;
            case "RIGHT":
                ik_target = ik_target_arm2;
                ik2D_arm = ik2D_arm2;
                targetReference = targetReference_arm2;
                followRight = true;
                left = false;
                break;
            default:
                yield break;
        }

		bool instant = false;
		if (targetTransform.GetComponent<FalconTarget> () != null) {
			targetTransform.GetComponent<FalconTarget> ().Set(targetReference.position,
			                                                  targetReference.rotation);
			instant = true;
		}

        ik2D_arm.IsEnabled = true;

        float maxDegreesPerSecond = 45.0f;
        float maxSpeed = referenceBase.TransformVector(0.25f, 0, 0).magnitude;

        while (left && followLeft || !left && followRight) {
			ik_target.position = Vector3.MoveTowards(targetReference.position, targetTransform.position, instant ? float.PositiveInfinity : maxSpeed * Time.deltaTime);
			ik_target.rotation = Quaternion.RotateTowards(targetReference.rotation, targetTransform.rotation, instant ? float.PositiveInfinity : maxDegreesPerSecond * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        ik2D_arm.IsEnabled = false;
    }

    void Unfollow(string side) {
        switch (side) {
        case "LEFT":
            followLeft = false;
            break;
        case "RIGHT":
            followRight = false;
            break;
        }
    }
	
	IEnumerator WaitUntilReached(string side, Transform targetTransform, float waitUntilReachedThreshold) {
		Unfollow(side);

		Transform ik_target;
		ik2D ik2D_arm;
		Transform targetReference;
		bool left;
		
		switch (side) {
		case "LEFT":
			ik_target = ik_target_arm1;
			ik2D_arm = ik2D_arm1;
			targetReference = targetReference_arm1;
			followLeft = true;
			left = true;
			break;
		case "RIGHT":
			ik_target = ik_target_arm2;
			ik2D_arm = ik2D_arm2;
			targetReference = targetReference_arm2;
			followRight = true;
			left = false;
			break;
		default:
			yield break;
		}

		Vector3 v;
		float d;

		GameObject targetSphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		targetSphere.transform.parent = referenceBase;
		targetSphere.transform.position = targetTransform.position;
		targetSphere.transform.localScale = Vector3.one * waitUntilReachedThreshold;
		MeshRenderer renderer = targetSphere.GetComponent<MeshRenderer> ();
		renderer.material.color = Color.red;


		while (left && followLeft || !left && followRight) {
			v = targetReference.position - targetTransform.position;
			d = Vector3.Dot(v, targetTransform.forward);

			float distance = referenceBase.InverseTransformVector(v).magnitude;
			if (distance < waitUntilReachedThreshold
			    || (d > 0 && referenceBase.InverseTransformVector(targetReference.position - targetTransform.position + targetTransform.forward * d).magnitude < waitUntilReachedThreshold))
				break;

			NearingSound(distance, 0.4f);

			yield return new WaitForFixedUpdate();
		}

		Destroy (targetSphere);

		if (left) {
			followLeft = false;
		} else {
			followRight = false;
		}
	}
	
	IEnumerator WaitUntilMovedTo(Transform targetTransform, float waitUntilReachedThreshold) {
		GameObject targetSphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		targetSphere.transform.parent = referenceBase;
		targetSphere.transform.position = targetTransform.position;
		targetSphere.transform.localScale = Vector3.one * waitUntilReachedThreshold;
		MeshRenderer renderer = targetSphere.GetComponent<MeshRenderer> ();
		renderer.material.color = Color.red;
		
		Vector3 v ;
		do {
			v = targetReference.position - targetTransform.position;

			yield return new WaitForFixedUpdate ();
		} while (v.magnitude > waitUntilReachedThreshold);
		
		Destroy (targetSphere);
	}

	public void SetFalconActive(bool this_falcon_active){
		arrow.gameObject.SetActive (this_falcon_active);
		reference.gameObject.SetActive (this_falcon_active);
	}

    public Vector3 FalconTipPosition(Vector3 position) {
		//print(position);
		direction = Vector3.zero;

		float mx = Mathf.Abs (position.x);
		float my = Mathf.Abs (position.y);
		float mz = Mathf.Abs (position.z);
		float max = Mathf.Max(mx, my, mz);

		if(mx == max) // crane
		{
			if (position.x < 0)
			{
				direction = new Vector3(-1 * mx, 0, 0);
				arrow.localEulerAngles = new Vector3(0,0,0);
			}
			else
			{
				direction = new Vector3(1 * mx, 0, 0);
				arrow.localEulerAngles = new Vector3(0, 0, 180);
			}
		}
		else if (my == max) // pulley
		{
			if (position.y < 0)
			{
				direction = new Vector3(0, 0, -1 * my);
				arrow.localEulerAngles = new Vector3(0, 0, -270);
			}
			else
			{
				direction = new Vector3(0, 0, 1 * my);
				arrow.localEulerAngles = new Vector3(0, 0, -90);
			}
		}
		else if (mz == max) // wagon
		{
			if (position.z < 0)
			{
				direction = new Vector3(0, 1 * mz, 0);
				arrow.localEulerAngles = new Vector3(0, 270, 0);
			}
			else
			{
				direction = new Vector3(0, -1 * mz, 0);
				arrow.localEulerAngles = new Vector3(0, 90, 0);
			}
		}
		// return force feedback
		return Vector3.zero;
    }

    public void FalconButtonPressed(int i) {
		//print(i + " pressed");

		if (i == 2) {
			falcon_translate = true;
		}
		else if (i == 1) {
			falcon_rotate_left = true;
		}
		else if (i == 3) {
			falcon_rotate_right = true;
		}
    }

    public void FalconButtonReleased(int i) {
		//print(i + " released");
		if (i == 2)
		{
			falcon_translate = false;
		}
		else if (i == 1) {
			falcon_rotate_left = false;
		}
		else if (i == 3) {
			falcon_rotate_right = false;
		}
    }

	void NearingSound(float distance, float fallOffDistance) {
		if (distance <= fallOffDistance && sinceLastNearingSound >= distance / fallOffDistance) {
			sinceLastNearingSound = 0;
			nearingSource.pitch = Mathf.Clamp(fallOffDistance / distance - 0.5f, 0.5f, 2f);
			nearingSource.PlayOneShot(nearingSound);
		}
	}

}
