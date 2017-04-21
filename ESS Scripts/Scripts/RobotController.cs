using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour {

	public GameObject claw1;
	public GameObject claw2;
	public GameObject shoulder;
	public GameObject upArm;
	public GameObject arm;
	public GameObject wrist;
	public GameObject handTarget;
	public GameObject elbowTarget;
	public float minAngle_UpArm_ForeArm = 40;
	public float minAngle_ForeArm_Wrist = 80;

	private float speed;
	private float startingTime;
	private float elev_startTime;
	private bool elev_btnPressed = false;
	private float totalTime;
	private float elev_totalTime = 2.0f;
	private float LClaw_dist;
	private Vector3 Lclaw1_opened_pos;
	private Vector3 Lclaw2_opened_pos;
	private Vector3 Lclaw1_closed_pos;
	private Vector3 Lclaw2_closed_pos;
	private Vector3 Lclaw1_last_pos;
	private Vector3 Lclaw2_last_pos;
	private float rangeDist;

	private Vector3 elev1UpPos;
	private Vector3 elev1DownPos;
	private Vector3 elev2UpPos;
	private Vector3 elev2DownPos;
	private Vector3 elev3UpPos;
	private Vector3 elev3DownPos;

	private bool closing;
	private bool openHold;
	private bool grip_active;
	
	void Start () {
		speed = 10;
		openHold = false;
		LClaw_dist = Vector3.Distance(claw1.transform.position, claw2.transform.position);
		Lclaw1_opened_pos = claw1.transform.localPosition;
		Lclaw2_opened_pos = claw2.transform.localPosition;

		Lclaw1_closed_pos = claw1.transform.localPosition;
		Lclaw1_closed_pos.z = Lclaw1_closed_pos.z + ((LClaw_dist / 2) - 0.36f);
		Lclaw2_closed_pos = claw2.transform.localPosition;
		Lclaw2_closed_pos.z = Lclaw2_closed_pos.z - ((LClaw_dist / 2) - 0.36f);

		closing = true;
		grip_active = false;
		totalTime = 1.0f;
		rangeDist = Vector3.Distance (shoulder.transform.position, handTarget.transform.position);
	}

	void Update () {
		if (Input.GetKey ("left")) {
			Vector3 tmp = handTarget.transform.localPosition + new Vector3(0,0,-speed * Time.deltaTime);
			float angleAA = Vector3.Angle(upArm.transform.up, arm.transform.forward);
			float angleAW = Vector3.Angle(-arm.transform.forward, -wrist.transform.up);

			if(angleAA > minAngle_UpArm_ForeArm && angleAW > minAngle_ForeArm_Wrist){
				handTarget.transform.localPosition = tmp;
			}
		}

		if (Input.GetKey ("right")) {

			Vector3 tmp = handTarget.transform.localPosition + new Vector3(0,0,speed * Time.deltaTime);
			float angleAA = Vector3.Angle(upArm.transform.up, arm.transform.forward);
			float angleAW = Vector3.Angle(-arm.transform.forward, -wrist.transform.up);

			if(angleAA > minAngle_UpArm_ForeArm && angleAW > minAngle_ForeArm_Wrist){
				handTarget.transform.localPosition = tmp;
			}
		}

		if (Input.GetKey ("down")) {

			Vector3 tmp = handTarget.transform.localPosition + new Vector3(speed * Time.deltaTime,0,0);
			float angleAA = Vector3.Angle(upArm.transform.up, arm.transform.forward);
			float angleAW = Vector3.Angle(-arm.transform.forward, -wrist.transform.up);

			if(angleAA > minAngle_UpArm_ForeArm && angleAW > minAngle_ForeArm_Wrist && 
			   handTarget.transform.localPosition.x < elbowTarget.transform.localPosition.x){
				handTarget.transform.localPosition = tmp;
			}
		}

		if (Input.GetKey ("up")) {
			Vector3 tmp = handTarget.transform.localPosition + new Vector3(-speed * Time.deltaTime,0,0);
				handTarget.transform.localPosition = tmp;
		}

		if (Input.GetKey ("u")) {
			shoulder.transform.Rotate(new Vector3(0,0,-speed * Time.deltaTime));
		}

		if (Input.GetKey ("o")) {
			shoulder.transform.Rotate(new Vector3(0,0, speed * Time.deltaTime));
		}

		if (Input.GetKey ("p") || grip_active) {
			if(!grip_active){
				grip_active = true;
				startingTime = Time.fixedTime;
			}

			if((!claw1.GetComponent<LClaw1> ().isHoldingSth || !claw2.GetComponent<LClaw2> ().isHoldingSth) && !openHold){
				float t = Mathf.Clamp ((Time.fixedTime - startingTime) / totalTime, 0, 1);

				Lclaw1_last_pos = Vector3.Lerp (closing ? Lclaw1_opened_pos : Lclaw1_closed_pos,
				                                closing ? Lclaw1_closed_pos : Lclaw1_opened_pos,
				                                t);
				claw1.transform.localPosition = Lclaw1_last_pos;

				Lclaw2_last_pos = Vector3.Lerp (closing ? Lclaw2_opened_pos : Lclaw2_closed_pos,
				                                closing ? Lclaw2_closed_pos : Lclaw2_opened_pos,
				                                t);
				claw2.transform.localPosition = Lclaw2_last_pos;

				if(t == 1){
					grip_active = false;
					closing = !closing;
				}
			} else if(!closing){
				float t = Mathf.Clamp ((Time.fixedTime - startingTime) / totalTime, 0, 1);
				
				claw1.transform.localPosition = Vector3.Lerp (Lclaw1_last_pos,
				                                              Lclaw1_opened_pos,
				                                              t);
				claw2.transform.localPosition = Vector3.Lerp (Lclaw2_last_pos,
				                                              Lclaw2_opened_pos,
				                                              t);
				if(t == 1){
					grip_active = false;
					closing = !closing;
					openHold = false;
					claw1.GetComponent<LClaw1> ().letGo();
					claw2.GetComponent<LClaw2> ().letGo();
				}
			}else {
				grip_active = false;
				claw1.GetComponent<LClaw1> ().dontLetGo();
				claw2.GetComponent<LClaw2> ().dontLetGo();
				Lclaw1_last_pos = claw1.transform.localPosition;
				Lclaw2_last_pos = claw2.transform.localPosition;
				openHold = true;
				if(closing)
					closing = false;
			} 
		}
	}
}
