using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movable))]
public class CraneOld : MonoBehaviour {
	
	public Material defaultMaterial;

	public KeyCode moveForwards = KeyCode.T;
	public KeyCode moveBackwards = KeyCode.G;
    public KeyCode moveWagonRight = KeyCode.H;
    public KeyCode moveWagonLeft = KeyCode.F;
    public KeyCode movePulleyUp = KeyCode.R;
    public KeyCode movePulleyDown = KeyCode.Y;
    
    private Movable movable;
	public Movable wagon;
	public Movable pulley;

	public Transform cable;
	private Vector3 cableStartPosition;

	void Start() {
        movable = GetComponent<Movable>();
        
		if (cable == null) {
			cable = transform.Find("/Block:_338");
		}
		cableStartPosition = cable.localPosition;

		Paint();
	}
	
	void Paint() {
		Transform[] children = transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform child in children) {
			Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers) {
				renderer.material = defaultMaterial;
			}
		}
	}

	void LateUpdate() {
		if (Input.GetKey(moveForwards)) {
            movable.Move(1);
		} else if (Input.GetKey(moveBackwards)) {
            movable.Move(-1);
		}

		if (Input.GetKey(moveWagonRight)) {
			wagon.Move(1);
		} else if (Input.GetKey(moveWagonLeft)) {
			wagon.Move(-1);
		}

		if (Input.GetKey(movePulleyUp)) {
			pulley.Move(1);
		} else if (Input.GetKey(movePulleyDown)) {
			pulley.Move(-1);
		}
        
        cable.localPosition = cableStartPosition - pulley.offset;
        cable.localScale = Vector3.one - pulley.offset / 2;
    }

}
