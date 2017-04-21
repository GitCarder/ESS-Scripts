using UnityEngine;

public class WindowDepthCamera : MonoBehaviour {

    public Material depthTextureMaterial;
    public RenderTexture renderTexture;
    public RenderTexture depthTexture {
        get {
            return renderTexture;
        }
    }

    void Start () {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, depthTextureMaterial);
    }
    
}
