using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class Sequencer : Callable {

    public SequenceList activeSequenceList;
    private List<SequenceList> sequenceLists = new List<SequenceList>();
	private int currentSequence_ix = 0;
	private AudioSource audiosource;
	public AudioClip beepSound;

    public List<SequenceList> sequences {
        get {
            return sequenceLists;
        }
    }

    public SequenceList currentMission {
        get {
            return activeSequenceList;
        }
    }

    public Sequence currentSequence {
        get {
            return (activeSequenceList != null && currentSequence_ix < activeSequenceList.Count) ? activeSequenceList[currentSequence_ix] : null;
        }
    }
	
	public int currentSequenceIndex {
		get {
			return currentSequence_ix;
		}
	}

    void Start() {
		audiosource = GetComponent<AudioSource> ();

		string[] filePaths = Directory.GetFiles(Application.dataPath + "/Sequences", "*.seq");
		foreach (string filePath in filePaths) {
			int dirSepIx = filePath.LastIndexOf(Path.DirectorySeparatorChar);
			sequenceLists.Add(new SequenceList(filePath.Substring(dirSepIx + 1, filePath.Length - 5 - dirSepIx)));

			string[] instructions = System.IO.File.ReadAllLines(filePath);
			foreach (string instruction in instructions) {
				string trimmed = instruction.Trim();
				if (trimmed.Length == 0 || trimmed.StartsWith("//"))
					continue;

				int indentations = instruction.Length - instruction.TrimStart('\t').Length;

				switch (indentations) {
				case 0:
					sequenceLists[sequenceLists.Count - 1].Add(new Sequence(trimmed));
					break;
				case 1:
					string[] parts = trimmed.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    string[] arguments = null;
					if (parts.Length >= 3) {
						arguments = new string[parts.Length - 2];
						for (int i = 0; i < arguments.Length; i++) {
							arguments[i] = parts[i + 2];
						}
					}
					SequenceList sequenceList = sequenceLists[sequenceLists.Count - 1];
                    bool block = !parts[1].StartsWith("!");
                    sequenceList.Last().Add(new Action(parts[0], parts[1].Substring(block ? 0 : 1), arguments, block));
					break;
				}
			}
		}
    }

    public override IEnumerator Call(string method, string[] arguments, bool block) {
		switch (method) {
            case "WAIT":
                yield return new WaitForSeconds(float.Parse(arguments[0]));
                break;
			case "BLINK":
				GameObject obj = GameObject.Find(arguments[0]);
				bool shh = arguments.Length >= 2 ? arguments[1].ToLower().Equals("shh") : false;
				int blinks = arguments.Length >= 3 ? int.Parse(arguments[2]) : 3;
				float duration = arguments.Length >= 4 ? float.Parse(arguments[3]) : 1;

				if (block) {
					yield return StartCoroutine(Blink(obj, blinks, duration, shh));
				} else {
					StartCoroutine(Blink(obj, blinks, duration, shh));
				}
				break;
        }
    }
	
	IEnumerator Blink(GameObject obj, int blinks, float duration, bool shh) {
		float totalDuration = blinks * duration;
		Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
		Dictionary<Material, Color> materials = new Dictionary<Material, Color>();
		foreach (Renderer renderer in renderers) {
			if (!materials.ContainsKey(renderer.material))
				materials.Add(renderer.material, renderer.material.color);
		}

		int bix = 0;
		for (float t = 0, lt = 1; t <= totalDuration; t += Time.deltaTime) {
			float s = Mathf.SmoothStep(0.5f, 1, (t % duration) / duration);

			if (!shh) {
				if (t % duration < lt) {
					audiosource.PlayOneShot(beepSound);
					bix++;
				}
				lt = t % duration;
			}

			// do magic
			foreach (Material material in materials.Keys) {
				material.color = Color.Lerp(Color.blue, materials[material], s);
			}

			yield return new WaitForFixedUpdate();
		}
	}

    void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            activeSequenceList = sequenceLists[2];
            activeSequenceList[currentSequence_ix++].Go();
        }
    }

    void OnGUI() {
		if (activeSequenceList == null) {
			for (int i = 0; i < sequenceLists.Count; i++) {
				if (GUI.Button(new Rect(10, 10 + 50 * i, 300, 40), sequenceLists[i].title)) {
                    SetSequenceList(i);
                }
			}
		} else {
			if (currentSequence_ix > 0
                && currentSequence_ix < activeSequenceList.Count
                && activeSequenceList[currentSequence_ix].IsActive()) {
				GUI.Label(new Rect(10, 10, 300, 40), "ACTIVE SEQUENCE: " + activeSequenceList[currentSequence_ix - 1].title);
			} else if (currentSequence_ix < activeSequenceList.Count
                && !activeSequenceList[currentSequence_ix].IsActive()) {
				if (GUI.Button(new Rect(10, 10, 300, 40), "LAUNCH: " + activeSequenceList[currentSequence_ix].title)) {
                    LaunchNext();
                }
			} else if (currentSequence_ix >= activeSequenceList.Count) {
				activeSequenceList = null;
                currentSequence_ix = 0;
			}
		}
	}

    public bool SetSequenceList(int ix) {
        if (ix >= 0 && ix < sequenceLists.Count && activeSequenceList == null) {
            activeSequenceList = sequenceLists[ix];
            currentSequence_ix = 0;
            StartCoroutine(Go());
            return true;
        }
        return false;
    }

    public bool LaunchNext() {
		if (currentSequence_ix > 0 && !activeSequenceList[currentSequence_ix - 1].IsActive() && currentSequence_ix < activeSequenceList.Count) {
			StartCoroutine(Go());
            return true;
        } else {
            return false;
        }
    }

    private IEnumerator Go() {
        yield return StartCoroutine(activeSequenceList[currentSequence_ix].Go());
        currentSequence_ix++;
        if (currentSequence_ix == activeSequenceList.Count) {
            activeSequenceList = null;
            currentSequence_ix = 0;
        }
    }

    public class SequenceList : List<Sequence> {

		public string title;
		
		public SequenceList(string title) {
			this.title = title;
            Add(new Sequence("Initializing sequence"));
		}

		public Sequence Last() {
			return this [Count - 1];
		}

	}

    public class Sequence {

		public string title;
		private List<Action> actions = new List<Action>();
		private bool active = false;

		public Sequence(string title) {
			this.title = title;
		}

		public void Add(Action action) {
			actions.Add (action);
		}

		public IEnumerator Go() {
			active = true;
			foreach (Action action in actions) {
				yield return StartCoroutine(action.Go());
			}
			active = false;
		}

		public bool IsActive() {
			return active;
		}

	}

    public class Action {

		public string target;
		public string method;
		public string[] arguments;
        public bool block;

		public Action(string target, string method, string[] arguments, bool block) {
			this.target = target;
			this.method = method;
			this.arguments = arguments;
            this.block = block;
		}

		public IEnumerator Go() {
			Callable[] callables = GameObject.Find (target).GetComponents<Callable>();
			foreach (Callable callable in callables)
				yield return StartCoroutine(callable.Call(method, arguments, block));
		}

	}
	
	public static Coroutine StartCoroutine(IEnumerator iterationResult) {
		return (new GameObject("Coroutine").AddComponent(typeof(CoroutinerFactory)) as CoroutinerFactory).Do(iterationResult);
	}

	public class CoroutinerFactory : MonoBehaviour {
		
		public Coroutine Do(IEnumerator iterationResult) {
			return StartCoroutine(DestroyWhenComplete(iterationResult));
		}
		
		public IEnumerator DestroyWhenComplete(IEnumerator iterationResult) {
			yield return StartCoroutine(iterationResult);
			Destroy(gameObject);
		}
		
	}

}
