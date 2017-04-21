using UnityEngine;
using System.Collections;

public class TranslateMenu : MonoBehaviour {   

    public Transform buttons;
    public Transform sliders;
    public Transform omni_slider;

    private ButtonDemoToggle[] i_buttons;

    private Transform[] i_sliders;
    private int active_slider;
	private bool any_slider_active;
	    
    void Start () {
        i_buttons = buttons.gameObject.GetComponentsInChildren<ButtonDemoToggle>();

        int count = sliders.childCount;        
        i_sliders = new Transform[count];    
        for(int i = 0; i < count; i++)
        {
            i_sliders[i] = sliders.GetChild(i);
            i_sliders[i].gameObject.SetActive(false);
        }
        active_slider = -1;
		any_slider_active = false;
        gameObject.SetActive(false);
    }

	void Update () {		        
		if (!any_slider_active) {
			bool any_button_active = false;

			for (int i = 0; i < i_buttons.Length; i++) {           
				if (i_buttons [i].ToggleState == true) {
					any_button_active = true;
					if (active_slider != i) {
						i_sliders [i].gameObject.SetActive (true);
						disablePrevButton (active_slider);
						active_slider = i;											           
				
						if (omni_slider != null) {
							omni_slider.gameObject.SetActive (true);
							omni_slider.GetComponent<CraneManouverOmni> ().setSlider (i);
						}									                    
						break;
					}
				} else if (i_buttons [i].ToggleState == false) {
					i_sliders [i].gameObject.SetActive (false);
					i_buttons [i].ButtonTurnsOff ();             
				}
			}      

			if (!any_button_active) {
				active_slider = -1;
				if (omni_slider != null) {
					omni_slider.gameObject.SetActive (false);
					omni_slider.GetComponent<CraneManouverOmni> ().setSlider (-1);
				}            
			}
		}

		if(omni_slider.gameObject.activeSelf && omni_slider.GetComponentInChildren<SliderDemo>().isPressed()){
			any_slider_active = true;
			hideMesh(i_sliders[active_slider]);
		} else if(omni_slider.gameObject.activeSelf && !omni_slider.GetComponentInChildren<SliderDemo>().isPressed()){
			any_slider_active = false;
			unHideMesh(i_sliders[active_slider]);
		} 

		if(active_slider >= 0 && i_sliders[active_slider].GetComponentInChildren<SliderDemo>().isPressed()){
			any_slider_active = true;
			hideMesh(omni_slider);
		} else if(active_slider >= 0 && !i_sliders[active_slider].GetComponentInChildren<SliderDemo>().isPressed()){
			any_slider_active = false;
			unHideMesh(omni_slider);
		}
	}

	void hideMesh(Transform mesh){
		Renderer[] rend = mesh.gameObject.GetComponentsInChildren<Renderer>();
		for(int i = 0; i < rend.Length; i++){
			rend[i].enabled = false;
		}
	}

	void unHideMesh(Transform mesh){
		Renderer[] rend = mesh.gameObject.GetComponentsInChildren<Renderer>();
		for(int i = 0; i < rend.Length; i++){
			rend[i].enabled = true;
		}
	}

    void disableOtherButtons(int index)
    {
        for (int i = 0; i < i_buttons.Length; i++)
        {
            if(i != index)
            {
                i_buttons[i].ToggleState = false;
                i_buttons[i].ButtonTurnsOff();
                i_sliders[i].gameObject.SetActive(false);
            }
        }
    }

	void disablePrevButton(int index){
		if(index >= 0){
			i_buttons[index].ToggleState = false;
			i_buttons[index].ButtonTurnsOff();
			i_sliders[index].gameObject.SetActive(false);
		}
	}

    public void defaultState()
    {
        for (int i = 0; i < i_buttons.Length; i++)
        {            
            i_buttons[i].ToggleState = false;
            i_buttons[i].ButtonTurnsOff();
            i_sliders[i].gameObject.SetActive(false);            
        }
		omni_slider.gameObject.SetActive (false);
    }

	public bool anySliderActive(){
		return any_slider_active;
	}
}
