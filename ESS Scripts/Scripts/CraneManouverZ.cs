using UnityEngine;
using System.Collections;

public class CraneManouverZ : MonoBehaviour {

    private Transform crane;
    private Transform wagon;
    private Transform slider_top;
    
    void Start () {
        crane = transform.GetComponentInParent<ModelInteractionScript>().objects[0];
        wagon = transform.GetComponentInParent<ModelInteractionScript>().objects[1];
        slider_top = transform.Find("Slider Top");

        Vector3 tmp = slider_top.position;
        tmp.z = wagon.position.z;
        slider_top.position = tmp;

        tmp = transform.position;
        tmp.x = crane.transform.position.x;
        transform.position = tmp;
    }

	void Update () {
        Vector3 tmp = transform.position;
        tmp.x = crane.transform.position.x;
        transform.position = tmp;
        
		if (transform.GetComponentInChildren<SliderDemo> ().isPressed ()) {
			Vector3 tmp1 = wagon.position;
			tmp1.z = slider_top.position.z;
			wagon.position = tmp1;        
		} else {
			Vector3 tmp2 = slider_top.position;
			tmp2.z = wagon.position.z;
			slider_top.position = tmp2;
		}
    }
}
