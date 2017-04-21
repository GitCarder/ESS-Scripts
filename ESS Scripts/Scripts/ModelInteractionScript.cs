using UnityEngine;
using System.Collections;

public class ModelInteractionScript : MonoBehaviour {

    public Transform[] objects;
    public Transform select_button;
    public Transform obj_menu;
    public Transform info_window;

    private CamerasTrackingEquipmentScript ctes;
    private ButtonDemoToggle select_bdt;
	void Start () {        
        select_bdt = select_button.GetComponentInChildren<ButtonDemoToggle>();
        info_window.gameObject.SetActive(false);

        ctes = transform.GetComponentInParent<CamerasTrackingEquipmentScript>();                
	}
	
	
	void Update () {
        if (select_bdt.ToggleState == true)
        {
            select_bdt.ToggleState = false;
            select_bdt.ButtonTurnsOff();
            select_button.gameObject.SetActive(false);
            obj_menu.gameObject.SetActive(true);
            info_window.gameObject.SetActive(true);
            ctes.SetObject(objects[objects.Length-1]);            
        }
        else if(!transform.Find("ObjectMenu").gameObject.activeSelf)
        {            
            select_button.gameObject.SetActive(true);
            obj_menu.gameObject.SetActive(false);
            info_window.gameObject.SetActive(false);
			ctes.SetObject(null);
        }
    }
}
