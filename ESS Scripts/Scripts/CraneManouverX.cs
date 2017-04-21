using UnityEngine;
using System.Collections;

public class CraneManouverX : MonoBehaviour
{

    private Transform crane;
	private Transform slider_top;

    void Start()
    {
        crane = transform.GetComponentInParent<ModelInteractionScript>().objects[0];
		slider_top = transform.Find("Slider Top");

        Vector3 tmp = slider_top.position;
        tmp.x = crane.position.x;
        slider_top.position = tmp;
    }

    void Update()
    {
		if (transform.GetComponentInChildren<SliderDemo> ().isPressed ()) {
			Vector3 tmp = crane.position;
			tmp.x = slider_top.position.x;
			crane.position = tmp;
		} else {
			Vector3 tmp = slider_top.position;
			tmp.x = crane.position.x;
			slider_top.position = tmp;
		}
    }
}
