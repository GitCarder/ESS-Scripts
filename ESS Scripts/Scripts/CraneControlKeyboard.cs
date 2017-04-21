using UnityEngine;
using System.Collections;

public class CraneControlKeyboard : MonoBehaviour {

	public Transform crane;
	public Transform wagon;
	public Transform pulley;
	public float speed = 1;

	void Update () {
		if (Input.GetKey ("up")) {
			wagon.Translate(new Vector3(0,-1,0) * Time.deltaTime * speed);
		} else if (Input.GetKey ("down")) {
			wagon.Translate(new Vector3(0,1,0) * Time.deltaTime * speed);
		} else if (Input.GetKey ("left")) {
			crane.Translate(new Vector3(-1,0,0) * Time.deltaTime * speed);
		} else if (Input.GetKey ("right")) {
			crane.Translate(new Vector3(1,0,0) * Time.deltaTime * speed);
		} else if (Input.GetKey ("-")) {
			pulley.Translate(new Vector3(0,0,1) * Time.deltaTime * speed);
		} else if (Input.GetKey (".")) {
			pulley.Translate(new Vector3(0,0,-1) * Time.deltaTime * speed);
		}
	}
}
