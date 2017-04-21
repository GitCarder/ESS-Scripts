using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RenderQueuePriority : MonoBehaviour {

	public int offset = 0;
	private static List<RenderQueuePriority> renderQueuePriorities = new List<RenderQueuePriority>();

	public void Start() {
		renderQueuePriorities.Add(this);
	}

	public static void Change() {
		foreach (RenderQueuePriority renderQueuePriority in renderQueuePriorities) {
			renderQueuePriority.Offset(renderQueuePriority.offset);
		}
	}
	
	public static void Reset() {
		foreach (RenderQueuePriority renderQueuePriority in renderQueuePriorities) {
			renderQueuePriority.Offset(0);
		}
	}

	protected void Offset(int o) {
		Transform[] children = transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform child in children) {
			Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers) {
				if (renderer.transform.GetComponent<RenderQueuePriority>() == null
				 || renderer.transform.GetComponent<RenderQueuePriority>() == this) {
					if (renderer.material.name == "TargetWheelShell")
						print ("what");
					renderer.material.renderQueue = renderer.material.shader.renderQueue + o;
				}
			}
		}
	}

}
