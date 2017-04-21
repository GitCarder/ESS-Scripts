using UnityEngine;
using System.Collections;

public class CraneManouverOmni : MonoBehaviour {

    private int curr_obj;
    private Transform this_slider_top;
    private Transform obj;
	private Transform slider_start;
	private Transform slider_end;
	private float slider_dist;

	private Transform bounds;
    private Vector3 obj_start_pos;
	private Transform bound_start;
	private Transform bound_end;
	private float bound_dist;

	private Vector3 bound_x;
	private Vector3 bound_y;
	private Vector3 bound_z;

	void Start () {
        this_slider_top = transform.Find("Slider Top");
		bounds = transform.GetComponentInParent<ModelInteractionScript>().objects[0].transform.parent.Find("Boundaries");
		bound_start = bounds.GetChild (0);
		slider_start = transform.Find ("SliderLowerLimit");
		slider_end = transform.Find ("SliderUpperLimit");
		slider_dist = Vector3.Magnitude (slider_end.localPosition - slider_start.localPosition);
    }

	void Update () {
		if (curr_obj >= 0) {
			if (transform.GetComponentInChildren<SliderDemo> ().isPressed ()) {
					float relative_obj = Mathf.Abs (this_slider_top.localPosition.x - slider_start.localPosition.x) / slider_dist;

					Vector3 tmp = obj.position;

					if (curr_obj == 0) {
						tmp.x = bound_start.position.x + relative_obj * bound_dist;

					} else if (curr_obj == 1) {
						tmp.z = bound_start.position.z + relative_obj * bound_dist;
					
					} else if (curr_obj == 2) {
						tmp.y = bound_start.position.y - relative_obj * bound_dist;
					} 
					obj.position = tmp;
			} else {
				setSlider (curr_obj);
			}
		}
	}

    public void setSlider(int s)
    {
		if (s >= 0) {
			curr_obj = s;        
			obj = transform.GetComponentInParent<ModelInteractionScript> ().objects [curr_obj];
			bound_end = bounds.GetChild (curr_obj+1);

			bound_dist = Vector3.Magnitude (bound_start.position - bound_end.position);
			float relative_slider = 0;
			float bx = Mathf.Abs (bound_end.localPosition.x);
			float bz = Mathf.Abs (bound_end.localPosition.z);
			float by = Mathf.Abs (bound_end.localPosition.y);
			float bMax = Mathf.Max (bx, by, bz);
			if (bx == bMax) {
				relative_slider = Mathf.Abs (obj.position.x - bound_start.position.x) / bound_dist;

			} else if (bz == bMax) {
				relative_slider = Mathf.Abs (obj.position.z - bound_start.position.z) / bound_dist;

			} else if (by == bMax) {
				relative_slider = Mathf.Abs (obj.position.y - bound_start.position.y) / bound_dist;
			} 
			Vector3 tmp = Vector3.zero;
			tmp.x = slider_start.localPosition.x + relative_slider * slider_dist;
			this_slider_top.localPosition = tmp;
		}
    }
}
