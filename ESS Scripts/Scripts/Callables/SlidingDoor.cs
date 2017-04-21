using UnityEngine;
using System.Collections;

public class SlidingDoor : Callable {
    
    public Transform door;
    private bool open = false;
    private float t = 0;
    private float speed = 0.25f;
    private Vector3 closed = Vector3.zero;
    private Vector3 opened = new Vector3(0, 4, 0);
    
    void LateUpdate() {
        t = Mathf.Clamp(t + (open ? 1 : -1) * speed * Time.deltaTime, 0, 1);
        door.localPosition = Vector3.Lerp(closed, opened, Mathf.SmoothStep(0, 1, t));
    }

    public override IEnumerator Call(string method, string[] arguments, bool block) {
        switch (method) {
            case "OPEN":
                open = true;
                while (block && t < 1)
                    yield return null;
                break;
            case "CLOSE":
                open = false;
                while (block && t > 0)
                    yield return null;
                break;
        }
    }

}
