using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crane : Callable, FalconListener {

    public Transform referenceBase;
    public Transform referenceOrigin;

    private Transform attachedTransform;

    public Material defaultMaterial;
    
	public Transform wagon;
	public Transform pulley;

    private Vector3 pulleyStartPosition;

	public Transform cable;
	private Vector3 cableStartPosition;
    public List<TextMesh> textMeshes;

    private Vector3 lastPosition;
    private Vector3 target;
    public Transform targetReference;

    //Falcon
    public Transform arrow;
    public float maxSpeed = 1.0f;
	private Vector3 direction = Vector3.zero;
	private Vector3 falconVelocity = Vector3.zero;
	private Vector3 falconAcceleration = new Vector3(1, 1, 1);
    private bool falcon_pressed = false;

	public float minX = -41.5f;
	public float maxX = -15.5f;
	public float minY = -12.4f;
	public float maxY = -1.5f;
	
	// Sound
	private AudioSource audiosource;
	private Vector3 lastPos;

    void Start() {
		if (cable == null) {
			cable = transform.Find("/Block:_338");
        }
        cableStartPosition = cable.localPosition;
        pulleyStartPosition = pulley.localPosition;

        target = targetReference.position;
        Paint();

		arrow.gameObject.SetActive (false);

		audiosource = GetComponent<AudioSource>();
	}
	
	void Paint() {
		Transform[] children = transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform child in children) {
			Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers) {
                if (renderer.tag.Equals("Interactable")) continue;
                renderer.sharedMaterial = defaultMaterial;
			}
		}
	}

	void LateUpdate() {
		falconVelocity *= 0.94f;
        if (falcon_pressed) {
            float max = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));
			
			if (Mathf.Abs(direction.x) == max) {
				falconVelocity.x += Mathf.Sign(direction.x) * falconAcceleration.x * Time.deltaTime;
			} else if (Mathf.Abs(direction.z) == max) {
				falconVelocity.y += Mathf.Sign(direction.z) * falconAcceleration.y * Time.deltaTime;
			} else if (Mathf.Abs(direction.y) == max) {
				falconVelocity.z -= Mathf.Sign(direction.y) * falconAcceleration.z * Time.deltaTime;
			}
        }
		
		transform.position += referenceBase.TransformVector(Vector3.right * falconVelocity.x * Time.deltaTime);
		pulley.position += referenceBase.TransformVector(Vector3.up * falconVelocity.y * Time.deltaTime);
		wagon.position += referenceBase.TransformVector(Vector3.forward * falconVelocity.z * Time.deltaTime);

        // Restrict movement
        Vector3 l = transform.localPosition;
        l.x = Mathf.Clamp(l.x, minX, maxX);
        transform.localPosition = l;

        l = pulley.transform.localPosition;
        l.z = Mathf.Clamp(l.z, minY, maxY);
        pulley.transform.localPosition = l;

        l = wagon.transform.localPosition;
        l.y = Mathf.Clamp(l.y, -5.3f, 5.3f);
        wagon.transform.localPosition = l;

        // Scale cable
        cable.localPosition = cableStartPosition - (pulley.localPosition - pulleyStartPosition);
        cable.localScale = Vector3.one - (pulley.localPosition - pulleyStartPosition) / 2;
		
		// Sounds
		if (Time.fixedTime > 1) {
			Vector3 newPos = referenceBase.InverseTransformPoint (targetReference.position);
			float currSpeed = ((newPos - lastPos).magnitude / Time.deltaTime) / maxSpeed;
			audiosource.volume = Mathf.Lerp (audiosource.volume, Mathf.Min (1, currSpeed), 0.5f);
			audiosource.pitch = 0.75f + audiosource.volume / 4;
			lastPos = newPos;
		}
    }

    public override IEnumerator Call(string method, string[] arguments, bool block) {
        switch (method) {
            case "SET_POSITION":
                transform.position = float.Parse(arguments[0]) * Vector3.right;
                pulley.transform.position = float.Parse(arguments[1]) * Vector3.up;
                wagon.transform.position = float.Parse(arguments[2]) * Vector3.forward;
                break;
            case "MOVE_TO":
                Vector3 current = referenceBase.InverseTransformPoint(targetReference.position) - referenceOrigin.localPosition;
                float x = arguments[0].Equals("*") ? current.x : -float.Parse(arguments[0]);
                float y = arguments[1].Equals("*") ? current.y : float.Parse(arguments[1]);
                float z = arguments[2].Equals("*") ? current.z : float.Parse(arguments[2]);
                Vector3 target = referenceOrigin.position + referenceBase.TransformVector(x, y, z);
                
                if (block) {
                    yield return StartCoroutine(MoveTo(target));
                } else {
                    StartCoroutine(MoveTo(target));
                }

                break;
            case "ATTACH":
                Deattach();
                attachedTransform = GameObject.Find(arguments[0]).transform;
                attachedTransform.parent = pulley;
                break;
            case "DEATTACH":
                Deattach();
				break;
			case "WAIT_FOR":
				switch (arguments[0].ToUpper()) {
				case "RIGHT":
					while (!(falcon_pressed && direction.x == FalconMax())) {
						yield return new WaitForFixedUpdate();
					}
					break;
				case "INWARDS":
					while (!(falcon_pressed && direction.y == -FalconMax()))
						yield return new WaitForFixedUpdate();
						break;
				case "DOWN":
					while (!(falcon_pressed && direction.z == -FalconMax()))
						yield return new WaitForFixedUpdate();
						break;
				}
				break;
        }
    }

	float FalconMax() {
		return Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));
	}

    IEnumerator MoveTo(Vector3 target) {
        Vector3 d;
        Vector3 lastPosition;
        do {
            d = referenceBase.InverseTransformVector(target - targetReference.position);
            lastPosition = targetReference.position;

            // Move to target
			transform.position += referenceBase.TransformVector(Mathf.Clamp(d.x, -1, 1) * maxSpeed * Time.deltaTime * Vector3.right);
			pulley.transform.position += referenceBase.TransformVector(Mathf.Clamp(d.y, -1, 1) * maxSpeed * Time.deltaTime * Vector3.up);
			wagon.transform.position += referenceBase.TransformVector(Mathf.Clamp(d.z, -1, 1) * maxSpeed * Time.deltaTime * Vector3.forward);

            yield return new WaitForFixedUpdate();
		} while (d.magnitude > 0.1f && referenceBase.InverseTransformVector(lastPosition - targetReference.position).magnitude > (maxSpeed * Time.deltaTime / 10));
    }

    private void Deattach() {
        if (attachedTransform != null) {
            attachedTransform.parent = null;
            attachedTransform = null;
        }
    }

	public void SetFalconActive(bool this_falcon_active){
		if (this_falcon_active)
			arrow.gameObject.SetActive (true);
		else
			arrow.gameObject.SetActive(false);
	}

	public Vector3 FalconTipPosition(Vector3 position) {
        direction = Vector3.zero;

		float mx = Mathf.Abs (position.x);
		float my = Mathf.Abs (position.y);
		float mz = Mathf.Abs (position.z);
        float max = Mathf.Max(mx, my, mz);

		if(mx == max){ // crane        
            if (position.x < 0){
                direction = new Vector3(-1 * mx, 0, 0);
                arrow.localEulerAngles = new Vector3(90,0,0);
            }
            else{
				direction = new Vector3(1 * mx, 0, 0);
                arrow.localEulerAngles = new Vector3(90, 180, 0);
            }
        }
        else if (my == max){ // pulley        
            if (position.y < 0){
				direction = new Vector3(0, 0, -1 * my);
                arrow.localEulerAngles = new Vector3(90, 270, 0);
            }
            else{
				direction = new Vector3(0, 0, 1 * my);
                arrow.localEulerAngles = new Vector3(90, 90, 0);
            }
        }
        else if (mz != 0){ // wagon        
            if (position.z < 0){
				direction = new Vector3(0, 1 * mz, 0);
                arrow.localEulerAngles = new Vector3(0, 90, -90);
            }
            else{
				direction = new Vector3(0, -1 * mz, 0);
                arrow.localEulerAngles = new Vector3(0, 90, 90);
            }
        }
		return Vector3.zero;
    }
	
	public void FalconButtonPressed(int i) {
		//print(i + " pressed");
		if(i == 2){
            falcon_pressed = true;
        }
	}

    public void FalconButtonReleased(int i){
        //print(i + " released");
        if (i == 2){
            falcon_pressed = false;
        }
    }
}
