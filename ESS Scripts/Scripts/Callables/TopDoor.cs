using UnityEngine;
using System.Collections;

public class TopDoor : Callable {    

    public Transform door1;
    public Transform door2;
    public Vector3 door1_opened;
    public Vector3 door2_opened;

    private Vector3 door1_closed;
    private Vector3 door2_closed;
	private AudioSource audiosource;
	
	public AudioClip rollingSound;
	public AudioClip slamSound;

	private bool open = false;
	private float t = 0;
	private float speed = 0.25f;

    void Start() {
        door1_closed = door1.localPosition;
        door2_closed = door2.localPosition;
		audiosource = GetComponent<AudioSource>();
    }

    void LateUpdate() {
        t = Mathf.Clamp(t + (open ? 1 : -1) * speed * Time.deltaTime, 0, 1);
        door1.localPosition = Vector3.Lerp(door1_closed, door1_opened, Mathf.SmoothStep(0, 1, t));
        door2.localPosition = Vector3.Lerp(door2_closed, door2_opened, Mathf.SmoothStep(0, 1, t));
    }

    public override IEnumerator Call(string method, string[] arguments, bool block) {
        switch (method) {
            case "OPEN":
                open = true;
				audiosource.clip = rollingSound;
				audiosource.Play();
				while (block && t < 1) {
                    yield return null;
				}
				audiosource.Stop();
				audiosource.PlayOneShot(slamSound);
                break;
            case "CLOSE":
				open = false;
				audiosource.clip = rollingSound;
				audiosource.Play();
                while (block && t > 0) {
					yield return null;
				}
				audiosource.Stop();
				audiosource.PlayOneShot(slamSound);
                break;
        }
    }
    
}
