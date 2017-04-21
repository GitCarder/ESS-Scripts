using UnityEngine;
using System.Collections;

public class SlidersBehavior : MonoBehaviour {

	void Start () {        
        int count = transform.childCount;        
        for(int i = 0; i < count; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
	}

	void Update () {
	
	}
}
