using UnityEngine;
using System.Collections;

public abstract class Callable : MonoBehaviour {

	public abstract IEnumerator Call (string method, string[] arguments, bool block);

}
