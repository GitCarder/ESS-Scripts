using UnityEngine;
using System.Collections;

public class CamerasTrackingEquipmentScript : MonoBehaviour {

    public Transform cameras;

    private Transform obj;
    private Camera[] cams;

	Quaternion targetLookAt;
	private float degreesPerSecond = 10;

	void Start () {
        cams = new Camera[cameras.childCount];
        for (int i = 0; i < cameras.childCount; i++)
        {
            cams[i] = cameras.GetChild(i).GetComponent<Camera>();
        }
	}

	void Update () {
        if (obj != null)
        {
            for (int i = 0; i < cams.Length; i++)
			{
				targetLookAt = Quaternion.LookRotation(obj.position - cams[i].transform.position, Vector3.up);
				cams[i].transform.rotation = Quaternion.RotateTowards(cams[i].transform.rotation, targetLookAt, degreesPerSecond * Time.deltaTime);
            }
        }
	}

    public void SetObject(Transform new_obj)
    {
        obj = new_obj;
    }
}
