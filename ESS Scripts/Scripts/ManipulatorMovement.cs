using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movable))]
public class ManipulatorMovement : MonoBehaviour {

    private Movable movable;
	public Movable wagon;

	void Start() {
        movable = GetComponent<Movable>();
	}

	void LateUpdate() {
		if (Input.GetKey("1")) {
			wagon.Move(-1);
		} else if (Input.GetKey("2")) {
			wagon.Move(1);
		}

		if (Input.GetKey("3")) {
			movable.Move(1);
		} else if (Input.GetKey("4")) {
            movable.Move(-1);
		}
	}

}
