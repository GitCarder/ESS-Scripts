using UnityEngine;
using System;

public class Console : MonoBehaviour {

    public TextMesh text;
    private string command = "";
    private bool ísEnabled = false;

	void Start() {
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab))
            ísEnabled = !ísEnabled;
        if (ísEnabled) {
            foreach (char c in Input.inputString) {
                switch (c) {
                    case '\b':
                        if (command.Length > 0)
                            command = command.Substring(0, command.Length - 1);
                        break;
                    case '\n':
                    case '\r':
                        Exec();
                        command = "";
                        break;
                    default:
                        command += c;
                        break;
                }
            }
        }
        text.text = ísEnabled ? "> " + command : "";
    }

    void Exec() {
        print("Executing > " + command);

        string[] parts = command.Trim().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
        string[] arguments = null;
        if (parts.Length >= 3)
        {
            arguments = new string[parts.Length - 2];
            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = parts[i + 2];
            }
        }
        bool block = !parts[1].StartsWith("!");

        StartCoroutine(GameObject.Find(parts[0]).GetComponent<Callable>().Call(parts[1].Substring(block ? 0 : 1), arguments, block));
    }

}
