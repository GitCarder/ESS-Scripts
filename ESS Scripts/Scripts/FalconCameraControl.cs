using UnityEngine;
using System.Collections;

public class FalconCameraControl : Callable, FalconListener {

    public Camera cam;
    public float scale;
    public float fovInDegreesMax = 180;
    public float fovInDegreesMin = 30;
    public float fovMax = 60;
    public float fovMin = 20;

    private bool cam_active = false;
    private bool falcon_active = false;
    private bool pan = false;
    private Vector3 pos;
    private Vector3 falcon_start_pos;
    private Vector3 dif;
    private Vector3 cam_start_rotation;
    private float cam_start_fov;
    //private bool button = false;
    private Vector3 direction = Vector3.zero;

    private float falcon_magnitude_movement = 0.5f;
    private float zoom = 40;
    private bool zoomIn = false;
    private bool zoomOut = false;

	private bool resetFalcon = false;

    void Start () {
        zoom = fovMax - fovMin;
        if (cam != null)
            cam_active = true;
	}
	
	public override IEnumerator Call(string method, string[] arguments, bool block) {
		switch (method) {
		case "WAIT_FOR_MOVEMENT":
			Vector3 refPos = pos;
			while ((refPos - pos).magnitude < 0.05f)
				yield return new WaitForFixedUpdate ();
			break;
		case "WAIT_FOR_FULL_LEFT":
			while (pos.x > -0.075f) {
				print (pos.x);
				yield return new WaitForFixedUpdate ();
			}
			break;
		case "WAIT_FOR_PANNING":
			while (!pan)
				yield return new WaitForFixedUpdate ();
			break;
		case "WAIT_FOR_ZOOM_IN":
			while (!zoomIn)
				yield return new WaitForFixedUpdate ();
			break;
		case "WAIT_FOR_ZOOM_OUT":
			while (!zoomOut)
				yield return new WaitForFixedUpdate ();
			break;
		}
	}
		
	void Update () {
        if (cam_active) {
            if (falcon_active)
			{                
				float x = (pos.x - falcon_start_pos.x);
				float y = (pos.y - falcon_start_pos.y);
				float z = (pos.z - falcon_start_pos.z);

                if (falcon_magnitude_movement != 0)
                {
                    Vector3 new_rot = new Vector3(cam_start_rotation.x - (y * (fovInDegreesMax / falcon_magnitude_movement)), cam_start_rotation.y + (x * (fovInDegreesMax / falcon_magnitude_movement)), 0);
                    cam.transform.localEulerAngles = new_rot;             
                }
            }

            if (zoomOut)
            {
                if (cam.fieldOfView < 70)
                {
                    cam.fieldOfView = cam.fieldOfView + 1;
                }
            }

            if (zoomIn)
            {
                if (cam.fieldOfView > 20)
                {
                    cam.fieldOfView = cam.fieldOfView - 1;
                }
            }

            if (pan)
            {                
                Vector3 tmp = cam.transform.localEulerAngles + direction * scale;
                tmp.z = 0;
                cam.transform.localEulerAngles = tmp;
                direction = new Vector3(-pos.y, pos.x, 0);                
            }
        }					      
    }

    public void SetCamera(Camera new_cam)
    {
        cam = new_cam;
		if (cam != null) {
			cam_active = true;
			SetCamStart ();
		} else
			cam_active = false;
    }

    public void SetFalconActive(bool this_falcon_active)
    {

        falcon_active = this_falcon_active;
        if (falcon_active)
        {
			if(cam != null){
				SetCamStart();	            
			}
        }
    }

    public Vector3 FalconTipPosition(Vector3 position)
	{
		if (resetFalcon) {
			falcon_start_pos = position;
			resetFalcon = false;
		}
        pos = position;
		return Vector3.zero;
    }

    public void FalconButtonPressed(int i)
    {        
        if (i == 2)
        {     
            falcon_active = false;
            pan = true;
            direction = new Vector3(-pos.y, pos.x, 0);            
			SetCamStart();
        }
        if (i == 1)
        {
            zoomOut = true;            
        }
        if (i == 3)
        {
            zoomIn = true;            
        }
    }

    public void FalconButtonReleased(int i)
    {		
        if (i == 2)
        {     
            falcon_active = true;
            pan = false;
			SetCamStart();
        }
        if(i == 1)
        {
            zoomOut = false;
        }
        if (i == 3)
        {
            zoomIn = false;
        }
    }

	void SetCamStart(){
		if (cam_active) {
			resetFalcon = true;
			cam_start_rotation = cam.transform.localEulerAngles;
			cam_start_fov = cam.fieldOfView;
		}
	}
}
