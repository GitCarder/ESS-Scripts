var bounds: Bounds;

// You could also declare these as an array, but they aren't generally
// used in a loop so it usually won't make much difference.
var topFrontLeft: Vector3;
var topFrontRight: Vector3;
var topBackLeft: Vector3;
var topBackRight: Vector3;
var bottomFrontLeft: Vector3;
var bottomFrontRight: Vector3;
var bottomBackLeft: Vector3;
var bottomBackRight: Vector3;


function Start() {
	var mf: MeshFilter = GetComponent(MeshFilter);
	bounds = mf.mesh.bounds;
	corners = new Vector3[8];
}

function UpdateCorners() {
	topFrontRight = transform.TransformPoint(bounds.center + bounds.extents);
	topFrontLeft = transform.TransformPoint(bounds.center + Vector3.Scale(bounds.extents, Vector3(-1, 1, 1)));
	topBackRight = transform.TransformPoint(bounds.center + Vector3.Scale(bounds.extents, Vector3(1, 1, -1)));
	topBackLeft = transform.TransformPoint(bounds.center + Vector3.Scale(bounds.extents, Vector3(-1, 1, -1)));
	bottomFrontRight = transform.TransformPoint(bounds.center + Vector3.Scale(bounds.extents, Vector3(1, -1, 1)));
	bottomFrontLeft = transform.TransformPoint(bounds.center + Vector3.Scale(bounds.extents, Vector3(-1, -1, 1)));
	bottomBackRight = transform.TransformPoint(bounds.center + Vector3.Scale(bounds.extents, Vector3(1, -1, -1)));
	bottomBackLeft = transform.TransformPoint(bounds.center + Vector3.Scale(bounds.extents, Vector3(-1, -1, -1)));
}

function Update () {
	UpdateCorners();
	Debug.DrawLine(topFrontLeft, topFrontRight);
	Debug.DrawLine(bottomFrontLeft, bottomFrontRight);
	Debug.DrawLine(topBackLeft, topBackRight);
	Debug.DrawLine(bottomBackLeft, bottomBackRight);
	Debug.DrawLine(topFrontLeft, topBackLeft);
	Debug.DrawLine(topFrontRight, topBackRight);
	Debug.DrawLine(bottomFrontLeft, bottomBackLeft);
	Debug.DrawLine(bottomFrontRight, bottomBackRight);
	Debug.DrawLine(topFrontLeft, bottomFrontLeft);
	Debug.DrawLine(topFrontRight, bottomFrontRight);
	Debug.DrawLine(topBackLeft, bottomBackLeft);
	Debug.DrawLine(topBackRight, bottomBackRight);
}