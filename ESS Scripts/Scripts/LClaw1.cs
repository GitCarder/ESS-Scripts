using UnityEngine;
using System.Collections;

public class LClaw1 : MonoBehaviour {

	public bool isHoldingSth;
	public GameObject hold;

	void Start () {
		isHoldingSth = false;
	}
		
	void OnCollisionEnter(Collision col){
		isHoldingSth = true;
		hold = col.gameObject;
	}

	void OnCollisionExit(Collision col){
		isHoldingSth = false;
		col.gameObject.transform.parent = null;
		col.gameObject.GetComponent<Rigidbody>().useGravity = true;
	}

	public void letGo(){
		hold.transform.parent = null;
		hold.GetComponent<Rigidbody> ().useGravity = true;
	}

	public void dontLetGo(){
		hold.transform.parent = this.transform.parent;
		hold.GetComponent<Rigidbody>().useGravity = false;
	}
}
