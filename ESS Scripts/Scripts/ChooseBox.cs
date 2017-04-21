using UnityEngine;
using System.Collections;

public class ChooseBox : MonoBehaviour {

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, transform.GetComponent<Renderer>().bounds.size);
	}
}
