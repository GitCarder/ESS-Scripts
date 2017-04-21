using UnityEngine;
using System.Collections;

public class CraneManouverY : MonoBehaviour {

    private Transform crane;
    private Transform wagon;
    private Transform pulley;
    private Transform slider_top;
	    
    void Start () {
        crane = transform.GetComponentInParent<ModelInteractionScript>().objects[0];
        wagon = transform.GetComponentInParent<ModelInteractionScript>().objects[1];
        pulley = transform.GetComponentInParent<ModelInteractionScript>().objects[2];
        slider_top = transform.Find("Slider Top");

        Vector3 tmp = slider_top.position;
        tmp.y = pulley.position.y;
        slider_top.position = tmp;

        tmp = transform.position;
        tmp.x = crane.position.x;
        tmp.z = wagon.position.z;
        transform.position = tmp;    
    }

	void Update () {
        Vector3 tmp = transform.position;
        tmp.x = crane.position.x;
        tmp.z = wagon.position.z;
        transform.position = tmp;

		if (transform.GetComponentInChildren<SliderDemo> ().isPressed ()) {
			Vector3 tmp1 = pulley.position;
			tmp1.y = slider_top.position.y;
			pulley.position = tmp1;
		} else {
			Vector3 tmp2 = slider_top.position;
			tmp2.y = pulley.position.y;
			slider_top.position = tmp2;
		}
    }
}
