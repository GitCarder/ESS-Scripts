using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionBriefing : Callable {

    private int currentMission_ix = 0;

    public Sequencer sequencer;

    public Text titleText;
    public Text briefingText;
	private float t = 0;
	private float speed = 1;
	private Color briefingTextColorA = Color.white;
	private Color briefingTextColorB = Color.white;

	public static bool space;
	public AudioClip notificationSound;
	private AudioSource audiosource;

	void Start() {
		audiosource = GetComponent<AudioSource>();
	}

	public override IEnumerator Call(string method, string[] arguments, bool block) {
		switch (method) {
		case "COLOR":
			if (arguments.Length >= 3) {
				briefingTextColorA = new Color(float.Parse(arguments[0]), float.Parse(arguments[1]), float.Parse(arguments[2]));
			}
			if (arguments.Length >= 6) {
				briefingTextColorB = new Color(float.Parse(arguments[3]), float.Parse(arguments[4]), float.Parse(arguments[5]));
			} else {
				briefingTextColorB = briefingTextColorA;
			}
			speed = arguments.Length >= 7 ? float.Parse(arguments[6]) : 1;
			yield return null;
			break;
		case "TEXT":
			briefingText.text = string.Join(" ", arguments);
			yield return null;
			break;
		case "CLEAR":
			briefingText.text = "";
			yield return null;
			break;
		case "TITLE":
			titleText.text = string.Join(" ", arguments);
			yield return null;
			break;
		case "CLEAR_TITLE":
			titleText.text = "";
			yield return null;
			break;
		}
	}

    void LateUpdate() {
		// Blink text color
		t = (t + Time.deltaTime / speed) % 1;
		briefingText.color = Color.Lerp(briefingTextColorA, briefingTextColorB, (Mathf.Sin(2 * Mathf.PI * Mathf.SmoothStep(0, 1, t)) + 1) / 2);

		space = space || Input.GetKeyDown (KeyCode.Space);
        if (sequencer.currentMission != null) {
			bool block = !sequencer.currentMission.title.Substring(0, 1).Equals("!");
			titleText.text = block ? sequencer.currentMission.title : sequencer.currentMission.title.Substring(1);
            
			if (!sequencer.currentSequence.IsActive() && space || !block) {
				audiosource.PlayOneShot(notificationSound);
                sequencer.LaunchNext();
            }
        } else {

			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				audiosource.PlayOneShot(notificationSound);
				currentMission_ix = (currentMission_ix - 1) % sequencer.sequences.Count;
            }
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				audiosource.PlayOneShot(notificationSound);
				currentMission_ix = (currentMission_ix + 1) % sequencer.sequences.Count;
            }
			if (Input.GetKeyDown(KeyCode.Space)) {
				audiosource.PlayOneShot(notificationSound);
                sequencer.SetSequenceList(currentMission_ix);
            }

			titleText.text = sequencer.sequences[currentMission_ix].title;
			briefingText.text = "Press space to start";
        }
		space = false;
	}

}
