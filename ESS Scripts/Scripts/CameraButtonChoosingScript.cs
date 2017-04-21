using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraButtonChoosingScript : Callable
{
	    
    public GameObject mainScreen;
    public GameObject minMon;
    public Material black_material;
    public Material monitor_material;

    public Transform buttons;
    public Transform cameras;
    public Transform monitors;
	public AudioClip monitorClick;

	private ButtonDemoToggle touchPad_button;
	    
    private Camera[] model_cameras;
    private ButtonDemoToggle[] camera_buttons;
    private GameObject[] interactive_mons;
    private int active_button;
    private int active_monitor;
    private int active;
    private bool both_active;
    private float rotSpeed = 15f;
	private AudioSource audio;

	private int indexWhenBlocked = -1;
	private bool modelEnabled = true;
	private bool monitorEnabled = true;

	public AudioClip tutorialWrongOne;
	private AudioSource tutorialAudioSource;

    void Start()
    {        		
        model_cameras = new Camera[cameras.childCount];
        for(int i = 0; i < cameras.childCount; i++)
        {
            model_cameras[i] = cameras.GetChild(i).GetComponent<Camera>();
        }
        
        interactive_mons = new GameObject[monitors.childCount];
        for (int i = 0; i < monitors.childCount; i++)
        {
			if (monitors.GetChild(i).gameObject.activeInHierarchy)
            	interactive_mons[i] = monitors.GetChild(i).gameObject;
        }
                  
        camera_buttons = buttons.gameObject.GetComponentsInChildren<ButtonDemoToggle>();        
        active_button = -1;
        active_monitor = -1;
        active = -1;
        both_active = false;
        minMon.SetActive(false);
        mainScreen.transform.Find("Canvas").Find("MidGraphics").Find("Main Screen").GetComponent<Renderer>().
        material.SetTexture("_MainTex", black_material.GetTexture("_MainTex"));                  

		audio = GetComponent<AudioSource>();

		tutorialAudioSource = gameObject.AddComponent<AudioSource> ();
    }

    void LateUpdate()
    {
		if (!modelEnabled || !monitorEnabled) {
			for (int i = 0; i < camera_buttons.Length; i++) {
				if (!modelEnabled && camera_buttons[i].ToggleState) {
					camera_buttons[i].ToggleState = false;
				}
				if (!monitorEnabled && interactive_mons[i].GetComponentInChildren<ButtonDemoToggle>().ToggleState) {
					interactive_mons[i].GetComponentInChildren<ButtonDemoToggle>().ToggleState = false;
				}
			}
		}
		if (!modelEnabled && !monitorEnabled) {
			return;
		}
			
        bool anyBtn = false;
        for(int i = 0; i < camera_buttons.Length; i++)
        {
			if((modelEnabled && camera_buttons[i].ToggleState) || (monitorEnabled && interactive_mons[i].GetComponentInChildren<ButtonDemoToggle>().ToggleState))
            {

                anyBtn = true;

                if (i != active)
                {					                   
                    mainScreen.transform.Find("Canvas").Find("MidGraphics").Find("Main Screen").GetComponent<Renderer>().
                        material.SetTexture("_MainTex", model_cameras[i].targetTexture);

					GetComponent<FalconCameraControl>().SetCamera(model_cameras[i]);
					audio.PlayOneShot(monitorClick);

                    camera_buttons[i].ButtonTurnsOn();
                    camera_buttons[i].ToggleState = true;
                    interactive_mons[i].GetComponentInChildren<ButtonDemoToggle>().ButtonTurnsOn();
                    interactive_mons[i].GetComponentInChildren<ButtonDemoToggle>().ToggleState = true;

					camera_buttons[i].transform.parent.GetComponentInChildren<SpriteFacer>().SetActive(true);

                    DisablePreviousButton(active);
                    DisablePreviousMonitor(active);
                    both_active = true;
                    active = i;
                    break;
                }               
            }
        }
    }
	

	public override IEnumerator Call(string method, string[] arguments, bool block) {
		switch (method) {
		case "ENABLE_MONITOR_INPUT":
			monitorEnabled = true;
			break;
		case "DISABLE_MONITOR_INPUT":
			monitorEnabled = false;
			break;
		case "WAIT_FOR_INPUT":
			indexWhenBlocked = active;
			while (active == indexWhenBlocked)
				yield return new WaitForFixedUpdate();
			break;
		case "ENABLE_MODEL_INPUT":
			modelEnabled = true;
			break;
		case "DISABLE_MONDEL_INPUT":
			modelEnabled = false;
			break;
		case "WAIT_FOR_CAM":
			int ix = int.Parse(arguments[0]);
			int lix = active;
			while (active != ix) {
				if (lix != active) {
					tutorialAudioSource.PlayOneShot(tutorialWrongOne);
					lix = active;
				}
				yield return new WaitForFixedUpdate();
			}
			break;
		}
	}
	 

    void DisablePreviousButton(int prev)
    {
        if(prev >= 0)
        {
            camera_buttons[prev].ButtonTurnsOff();
			camera_buttons[prev].ToggleState = false;
			camera_buttons[prev].transform.parent.GetComponentInChildren<SpriteFacer>().SetActive(false);
        }
    }

    void DisablePreviousMonitor(int prev)
    {
        if (prev >= 0)
        {
            interactive_mons[prev].GetComponentInChildren<ButtonDemoToggle>().ButtonTurnsOff();
            interactive_mons[prev].GetComponentInChildren<ButtonDemoToggle>().ToggleState = false;
        }
    }

    void DisableOtherButtons(int index)
    {
        for(int i = 0; i < camera_buttons.Length; i++)
        {
            if(i != index)
            {
                camera_buttons[i].ButtonTurnsOff();
                camera_buttons[i].ToggleState = false;
            }
        }
    }       
}
