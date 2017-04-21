using UnityEngine;
using System.Collections;

public class ObjectMenu : MonoBehaviour {

    public Transform closeButton;
    public Transform translateButton;
    public Transform translateMenu;

    private ButtonDemoToggle close_button;
    private ButtonDemoToggle translate_button;
    
    void Start () {
        close_button = closeButton.GetComponentInChildren<ButtonDemoToggle>();
        translate_button = translateButton.GetComponentInChildren<ButtonDemoToggle>();    
        gameObject.SetActive(false);
    }

	void Update () {
        if (close_button.ToggleState == true)
        {
            close_button.ToggleState = false;
            close_button.ButtonTurnsOff();
            TranslateMenu tm = translateMenu.GetComponentInChildren<TranslateMenu>();
            if(tm != null)
            {
                tm.defaultState();
            }
            translate_button.ToggleState = false;
            translate_button.ButtonTurnsOff();
            gameObject.SetActive(false);
        }

        if(translate_button.ToggleState == true)
        {
            translateMenu.gameObject.SetActive(true);
        }
        else if(translate_button.ToggleState == false)
        {
            TranslateMenu tm = translateMenu.GetComponentInChildren<TranslateMenu>();
            if (tm != null)
            {
                tm.defaultState();
            }
            translateMenu.gameObject.SetActive(false);
        }
	}
}

