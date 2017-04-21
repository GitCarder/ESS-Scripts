using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonitorChoosingScript : MonoBehaviour {


    public GameObject homeBtn;
    public GameObject monitorsPanel;
    public GameObject mainMonitor;
    public Transform miniCamPanel;

    private ButtonDemoToggle home_button;
    private ButtonDemoToggle[] monButtons;
    private List<GameObject> monitors = new List<GameObject>();
    private Transform originMonPanel; 

	void Start () {
        home_button = homeBtn.GetComponentInChildren<ButtonDemoToggle>();
        originMonPanel = monitorsPanel.transform;
        Debug.Log(monitorsPanel.transform.position);
        mainMonitor.SetActive(false);
        monitorsPanel.SetActive(true);
        monButtons = monitorsPanel.GetComponentsInChildren<ButtonDemoToggle>();
        miniCamPanel.gameObject.SetActive(false);

        foreach(Transform monitor in monitorsPanel.transform)
        {
            monitors.Add(monitor.gameObject);
        }    
    }

	void Update () {
        if (home_button.ToggleState == true)
        {
            mainMonitor.SetActive(false);    
            monitorsPanel.transform.position = originMonPanel.position;
            monitorsPanel.transform.localScale = originMonPanel.localScale;
            monitorsPanel.transform.rotation = originMonPanel.rotation;

            Debug.Log(originMonPanel.position);
            Debug.Log(monitorsPanel.transform.localPosition);

            home_button.ToggleState = false;
            home_button.ButtonTurnsOff();
        }

        for (int i = 0; i < monButtons.Length; i++){
            if (monButtons[i].ToggleState == true)
            {           
               // Debug.Log(i);
                GameObject mon = monitors[i].transform.Find("Canvas").transform.Find("MidGraphics").transform.Find("Monitor " + (i+1)).gameObject;

                Transform tmp_tr = mainMonitor.transform;
                GameObject tmp_go = mainMonitor;
                Destroy(tmp_go);
                mainMonitor = Instantiate(mon);
                
                mainMonitor.transform.position = tmp_tr.transform.position;
                mainMonitor.transform.rotation = tmp_tr.transform.rotation;
                mainMonitor.transform.localScale = tmp_tr.transform.localScale;

                monButtons[i].ToggleState = false;
                monButtons[i].ButtonTurnsOff();
				                
                monitorsPanel.transform.position = miniCamPanel.position;
                monitorsPanel.transform.localScale = miniCamPanel.localScale;
                monitorsPanel.transform.rotation = miniCamPanel.rotation;
                Debug.Log(monitorsPanel.transform.localPosition);
                break;
            }
        }

	}
}
