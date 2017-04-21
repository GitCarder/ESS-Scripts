using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class FalconController : Callable {
	
	public static readonly float SCALE = 1.5f;
	private Vector3 tipPosition;

    public List<FalconAdapter> adapters;
	public Transform falconInfoPanel;
	public Color monitorColor;
	public Color craneColor;
	public Color manipulatorColor;
	public Color defaultColor;
	public AudioClip switchSound;

    private float minDistToMaxForce = 0.0005f;
	private float maxDistToMaxForce = 0.01f;

    private bool[] buttons = new bool[4];
    private float initTime = 1;
    private float currentInitTime = 0;

	private int activeFalconAdapter = -1;
	private int monitorFalconAdapter = -1;
	private int prevActiveFalconAdapter = -1;
	private bool cameraControlActive = false;
	private AudioSource audio;


	// God object
	public FalconTarget falconTarget;

    void Start() {
        //FalconUnity.setForceField(0, -Vector3.forward * 5);

        adapters.AddRange(FindObjectsOfType<FalconAdapter>());

		// Init button array
        for (int i = 0; i < 4; i++) {
            buttons[i] = false;
        }

		print("Falcon adapters in scene: " + adapters.Count);

		FalconUnity.getTipPosition (0, out tipPosition);
		//FalconUnity.setSphereGodObject(0, 0.00001f, 0.001f, Vector3.zero, minDistToMaxForce * SCALE, maxDistToMaxForce * SCALE);
		FalconUnity.updateHapticTransform(0, Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1) * SCALE, false, 1/60.0f);

		for (int i = 0; i < adapters.Count; i++) {
			if (adapters[i].name.Equals("MONITOR")) {
				monitorFalconAdapter = i;
				break;
			}
		}

		falconInfoPanel.Find ("Panel").Find ("Title").GetComponent<Text> ().text = "";
		audio = GetComponent<AudioSource> ();
    }
	
	void Update() {
        if (currentInitTime > initTime) {
			if (!FalconUnity.getTipPosition(0, out tipPosition)) {
                return;
            }
			if (activeFalconAdapter != -1){
				Vector3 feedback = adapters[activeFalconAdapter].FalconTipPosition(tipPosition);
				
				FalconUnity.updateHapticTransform(0, Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1) * SCALE, false, Time.deltaTime);

				if (Input.GetKey(KeyCode.A)) {
					FalconUnity.applyForce(0, Vector3.forward, Time.deltaTime);
				}

				if (feedback != null) {
					//FalconUnity.applyForce(0, feedback, Time.deltaTime);
				}
			}
        } else if (currentInitTime < initTime) {
            currentInitTime = Mathf.Min(currentInitTime + Time.deltaTime, initTime);
        } else {
            //FalconUnity.setForceField(0, Vector3.zero);
			if (!FalconUnity.getTipPosition(0, out tipPosition)) {
                return;
            }
            currentInitTime = float.PositiveInfinity;
        }
    }
    
	void LateUpdate() {
        if (currentInitTime > initTime) {
            bool[] new_buttons = new bool[4];
            if (!FalconUnity.getFalconButtonStates(0, out new_buttons)) {
                return;
            }
            for (int i = 0; i < 4; i++) {
                if (buttons[i] ^ new_buttons[i]) {
                    if (new_buttons[i]) {
                        ButtonPressed(i);
                    } else {
                        ButtonReleased(i);
                    }
                }
                buttons[i] = new_buttons[i];
            }
        }
		if (Input.GetKeyDown(KeyCode.KeypadPlus)){
			SwitchFalcon((activeFalconAdapter + 1) % adapters.Count);
		} else if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
			int tmpInd = (activeFalconAdapter - 1) % adapters.Count;
			if(tmpInd < 0)
				tmpInd = adapters.Count - 1;
			SwitchFalcon(tmpInd);
		}
		for (int i = 0; i < 10; ++i) {
			if (Input.GetKeyDown("" + i)) {
				SwitchFalcon(i % adapters.Count);
			}
		}
	}
	
	void ButtonPressed(int i) {

		if (i == 0 && monitorFalconAdapter >= 0 && monitorFalconAdapter != activeFalconAdapter) {
			//cameraControlActive = true;
			prevActiveFalconAdapter = activeFalconAdapter;
			SwitchFalcon(monitorFalconAdapter);
		}
		else if (i == 0 && prevActiveFalconAdapter >= 0 && monitorFalconAdapter == activeFalconAdapter) {
			//cameraControlActive = false;
			SwitchFalcon(prevActiveFalconAdapter);
		}
		else if (activeFalconAdapter != -1)
			adapters[activeFalconAdapter].FalconButtonPressed(i);
	}

	void ButtonReleased(int i) {
		if (activeFalconAdapter != -1)
			adapters[activeFalconAdapter].FalconButtonReleased(i);
    }

	private void ReleaseButtons(int currentFalcon) {
		for (int i = 0; i < 4; i++) {
			adapters[currentFalcon].FalconButtonReleased(i);
		}
	}
	
	void SwitchFalcon(FalconAdapter newFalconAdapter) {
		if (activeFalconAdapter != -1) {
			ReleaseButtons (activeFalconAdapter);
			print ("FalconAdapter for " + adapters[activeFalconAdapter].gameObject.name + " is no longer active.");
			adapters [activeFalconAdapter].SetFalconActive (false);
		}
		for (int i = 0; i < adapters.Count; i++) {
			if (adapters[i] == newFalconAdapter) {
				activeFalconAdapter= i;
				print ("FalconAdapter for " + newFalconAdapter.gameObject.name + " is now active.");
				adapters [activeFalconAdapter].SetFalconActive (true);
				UpdateInfoPanel();
				return;
			}
		}
		// Not in list.
		print ("Error: FalconAdapter for " + newFalconAdapter.gameObject.name + " could not be activated.");
	}

	void SwitchFalcon(int newFalconAdapter) {
		if (activeFalconAdapter != -1) {
			ReleaseButtons (activeFalconAdapter);
			print ("FalconAdapter for " + adapters[activeFalconAdapter].gameObject.name + " is no longer active.");
			adapters [activeFalconAdapter].SetFalconActive (false);
		}
		activeFalconAdapter = newFalconAdapter;
		if (activeFalconAdapter != -1) {
			print ("FalconAdapter for " + adapters[activeFalconAdapter].gameObject.name + " is now active.");
			adapters [activeFalconAdapter].SetFalconActive (true);
			UpdateInfoPanel();
		}
	}

	void UpdateInfoPanel(){
		falconInfoPanel.Find ("Panel").Find ("Title").GetComponent<Text> ().text = "Now Controlling:";
		falconInfoPanel.Find("Panel").Find("Briefing text").GetComponent<Text>().text = adapters[activeFalconAdapter].displayName;
		audio.PlayOneShot (switchSound);

		if (adapters [activeFalconAdapter].name.Equals ("MONITOR")) {
			falconInfoPanel.Find ("Panel").Find ("Briefing text").GetComponent<Text> ().color = monitorColor;
		} else if (adapters [activeFalconAdapter].name.Equals ("CRANE_1") ||
			adapters [activeFalconAdapter].name.Equals ("CRANE_2")) {
			falconInfoPanel.Find ("Panel").Find ("Briefing text").GetComponent<Text> ().color = craneColor;
		} else if (adapters [activeFalconAdapter].name.Equals ("MANIPULATOR") ||
			adapters [activeFalconAdapter].name.Equals ("FALCON_TARGET")) {
			falconInfoPanel.Find ("Panel").Find ("Briefing text").GetComponent<Text> ().color = manipulatorColor;
		} else {
			falconInfoPanel.Find ("Panel").Find ("Briefing text").GetComponent<Text> ().color = defaultColor;			
		}
	}
	
	public override IEnumerator Call(string method, string[] arguments, bool block) {
		switch (method) {
		case "ACTIVATE":
			for (int i = 0; i < adapters.Count; i++) {
				if (adapters[i].name == arguments[0]) {				
					SwitchFalcon(adapters[i]);
					break;
				}
			}
			break;
		case "DEACTIVATE":
			SwitchFalcon(-1);
			break;
		case "WAIT_FOR_BUTTON":
			yield return StartCoroutine(WaitForButton(int.Parse(arguments[0])));
			break;
		}
		yield return new WaitForFixedUpdate();
	}

	IEnumerator WaitForButton(int button) {
		while (!buttons[button])
			yield return new WaitForFixedUpdate();
	}

}
