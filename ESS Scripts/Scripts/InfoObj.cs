using UnityEngine;
using System.Collections;

public class InfoObj : MonoBehaviour {

    private Transform obj;

    void Start()
    {
        obj = transform.GetComponentInParent<ModelInteractionScript>().objects[0];
    }
		    
    void Update()
    {
        Vector3 tmp = transform.position;
        tmp.x = obj.position.x;
        transform.position = tmp;
    }
}
