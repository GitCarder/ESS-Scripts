using UnityEngine;
using System.Collections;

public class ManElevatorController : MonoBehaviour {

	public GameObject elev_root;
	public GameObject elev_part1;
	public GameObject elev_part2;
	public GameObject elev_part3;
	public float rotationSpeed = 7.0f;

	private Vector3 elev1UpPos;
    private Vector3 elev2UpPos;
    private Vector3 elev3UpPos;
    private Vector3 elev1DownDir = -Vector3.up;
    private Vector3 elev2DownDir = Vector3.forward;
    private Vector3 elev3DownDir = Vector3.forward;
    private float elev1DownDistance = 0;
    private float elev2DownDistance = 0;
    private float elev3DownDistance = 0;
    private float elev1DownMaxDistance = 11.5f;
    private float elev2DownMaxDistance = 11.5f;
    private float elev3DownMaxDistance = 11.5f;
    private float speed = 10;
	    
    void Start () {
		elev1UpPos = elev_part1.transform.localPosition;
        elev2UpPos = elev_part2.transform.localPosition;
        elev3UpPos = elev_part3.transform.localPosition;
    }

	void Update () {
		if (Input.GetKey ("i")) {
            if (elev3DownDistance > 0) {
                elev3DownDistance = Mathf.Max(elev3DownDistance - speed * Time.deltaTime, 0);
            } else if (elev2DownDistance > 0) {
                elev2DownDistance = Mathf.Max(elev2DownDistance - speed * Time.deltaTime, 0);
            } else if (elev1DownDistance > 0) {
                elev1DownDistance = Mathf.Max(elev1DownDistance - speed * Time.deltaTime, 0);
            }
		} else if (Input.GetKey ("k")) {
            if (elev1DownDistance < elev1DownMaxDistance) {
                elev1DownDistance = Mathf.Min(elev1DownDistance + speed * Time.deltaTime, elev1DownMaxDistance);
            } else if (elev2DownDistance < elev2DownMaxDistance) {
                elev2DownDistance = Mathf.Min(elev2DownDistance + speed * Time.deltaTime, elev2DownMaxDistance);
            } else if (elev3DownDistance < elev3DownMaxDistance) {
                elev3DownDistance = Mathf.Min(elev3DownDistance + speed * Time.deltaTime, elev3DownMaxDistance);
            }
        }

        elev_part1.transform.localPosition = elev1UpPos + elev1DownDir * elev1DownDistance;
        elev_part2.transform.localPosition = elev2UpPos + elev2DownDir * elev2DownDistance;
        elev_part3.transform.localPosition = elev3UpPos + elev3DownDir * elev3DownDistance;

        if (Input.GetKey ("j")) {
			elev_root.transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
		} else if (Input.GetKey ("l")) {
			elev_root.transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
		}
	}
}
