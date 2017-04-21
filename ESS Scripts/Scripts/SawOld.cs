using UnityEngine;
using System.Collections;

public class SawOld : MonoBehaviour {

	public Material material;

	public KeyCode wagonRightKey = KeyCode.Z;
	public KeyCode wagonLeftKey = KeyCode.X;
	public KeyCode mountUpKey = KeyCode.C;
	public KeyCode mountDownKey = KeyCode.V;
	public KeyCode bladeMountUpKey = KeyCode.N;
	public KeyCode bladeMountDownKey = KeyCode.B;
	public KeyCode bladeUpKey = KeyCode.M;
	public KeyCode bladeDownKey = KeyCode.Comma;
	public KeyCode bladeOnKey = KeyCode.Period;
	
	public Movable wagon;
	public Movable mount;
	public Rotatable bladeMount;
	public Rotatable blade;

    void Start()  {
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			renderer.material = material;
		}
	}

	void LateUpdate() {
		if (Input.GetKey(wagonLeftKey)) {
			wagon.Move(-1);
        } else if (Input.GetKey(wagonRightKey)) {
            wagon.Move(1);
		}

		if (Input.GetKey(mountUpKey)) {
			mount.Move(1);
		} else if (Input.GetKey(mountDownKey)) {
            mount.Move(-1);
		}
		
		if (Input.GetKey(bladeMountUpKey)) {
			bladeMount.Rotate(1);
		} else if (Input.GetKey(bladeMountDownKey)) {
            bladeMount.Rotate(-1);
		}
		
		if (Input.GetKey(bladeUpKey)) {
            blade.Rotate(1);
		} else if (Input.GetKey(bladeDownKey)) {
            blade.Rotate(-1);
		}
    }
    
}
