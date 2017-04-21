using UnityEngine;
using System.Collections;

public class FalconRigidBody : MonoBehaviour {

	public float k = 0.5f;
 	public float mass = 1;
	public Vector3 device_scale = new Vector3(1.0f,1.0f,1.0f);//send 1/4 size in the z dimension
	public int bodyId = 0;
	public Vector3 linearFactors = new Vector3(1.0f,1.0f,1.0f);
	public Vector3 angularFactors = new Vector3(1.0f,1.0f,1.0f);
	public float friction = 0.8f;
	
	// Use this for initialization
	void Start () {
		bodyId = getNextBodyId();
	 	refreshShape();
	}
	
	public void refreshShape(){
		Mesh m = GetComponent<MeshFilter>().mesh;
   	 	Vector3[] v = m.vertices;
	 	int[] f = m.triangles;
	 	float[] shape = new float[f.Length*3];

		for(int i=0;i<f.Length;i++){
			Vector3 vert = v[f[i]];
			vert.Scale(transform.lossyScale);
			
			shape[i*3] = vert.x;
			shape[i*3+1] = vert.y;
			shape[i*3+2] = vert.z;
		}
		
		FalconUnity.sendDynamicShape(bodyId, shape, f.Length/3, mass, k, transform.position, transform.rotation, linearFactors, angularFactors, friction);
	}

	
	// Update is called once per frame
	public void FixedUpdate () {
		Vector3 pos;
		Quaternion orient;

		FalconUnity.getDynamicShapePose(bodyId, out pos, out orient);

		transform.position = pos;
		transform.rotation = orient;

		FalconUnity.updateDynamicShape(bodyId, mass, k, linearFactors, angularFactors, friction);
	}
	
	static object Lock = new object();
	private static int curId = -1;
	public static int getNextBodyId() {
		lock(Lock) {
			curId ++;
			return curId;
		}
	}
}
