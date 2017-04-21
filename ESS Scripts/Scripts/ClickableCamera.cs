using UnityEngine;
using System.Collections;
using LMWidgets;

public class ClickableCamera : ButtonBase {

    protected override void buttonPressed() {
        Debug.Log("press");
    }

    protected override void buttonReleased() {
        Debug.Log("release");
    }

    void LateUpdate () {
	}

}
