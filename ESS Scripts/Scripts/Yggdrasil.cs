using UnityEngine;

public class Yggdrasil : MonoBehaviour {

    public Window3D window3D;

	void Start() {
		//RenderQueuePriority.Change();
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
	}

	void OnPreRender() {
		Shader.SetGlobalVector("_TopPoint", new Vector4(window3D.top.position.x, window3D.top.position.y, window3D.top.position.z));
		Shader.SetGlobalVector("_TopNormal", new Vector4(window3D.top.up.x, window3D.top.up.y, window3D.top.up.z));
		Shader.SetGlobalVector("_TopHorizontal", new Vector4(window3D.top.right.x, window3D.top.right.y, window3D.top.right.z));
		Shader.SetGlobalVector("_BottomPoint", new Vector4(window3D.bottom.position.x, window3D.bottom.position.y, window3D.bottom.position.z));
		
		Shader.SetGlobalVector("_FrontPoint", new Vector4(window3D.front.position.x, window3D.front.position.y, window3D.front.position.z));
		Shader.SetGlobalVector("_FrontNormal", new Vector4(window3D.front.up.x, window3D.front.up.y, window3D.front.up.z));
		Shader.SetGlobalVector("_FrontHorizontal", new Vector4(window3D.front.right.x, window3D.front.right.y, window3D.front.right.z));
		Shader.SetGlobalVector("_BackPoint", new Vector4(window3D.back.position.x, window3D.back.position.y, window3D.back.position.z));
		
		Shader.SetGlobalVector("_LeftPoint", new Vector4(window3D.left.position.x, window3D.left.position.y, window3D.left.position.z));
		Shader.SetGlobalVector("_LeftNormal", new Vector4(window3D.left.up.x, window3D.left.up.y, window3D.left.up.z));
		Shader.SetGlobalVector("_LeftHorizontal", new Vector4(window3D.left.right.x, window3D.left.right.y, window3D.left.right.z));
		Shader.SetGlobalVector("_RightPoint", new Vector4(window3D.right.position.x, window3D.right.position.y, window3D.right.position.z));
		
		Shader.SetGlobalFloat("_Width", (window3D.left.position - window3D.right.position).magnitude);
		Shader.SetGlobalFloat("_Height", (window3D.top.position - window3D.bottom.position).magnitude);
		Shader.SetGlobalFloat("_Depth", (window3D.front.position - window3D.back.position).magnitude);

		Shader.SetGlobalInt ("_Yggdrasil", 1);
	}

	void OnPostRender() {
		Shader.SetGlobalInt ("_Yggdrasil", 0);
	}
    
}
