using UnityEngine;
using System.Collections;
using LMWidgets;

public class ScrollBaseTouchScreen : LeapPhysicsBase
{
    public float InteractionScale = 1.0f;

    // How strong is the snapping spring.
    public float SnapSpringForce = 100.0f;

    // How much drag should be applied ( as a percentage of velocity/second )
    public float Drag = 5.0f;

    // The transform of the root game object of the content to be scrolled.
    public Transform ContentTransform;

    // Used to define and top and bottom of the scroll content
    public Transform ContentTopBound;
    public Transform ContentBottomBound;

    // Used to define the top and bottom of the scroll container
    public Transform ContainerTopBound;
    public Transform ContainerBottomBound;


    // The current velocity of the content.
    protected float m_velocity = 0.0f;

    // The dampening force for the edge spring.
    protected float m_dampingForce = 0.0f;

    // The location of the scrollable content 
    // when interaction began.
    protected Vector3 m_contentPivot;

	public Transform touchScreen;
	public Material black_material;
	private Camera cam;
    private Vector3 camPivot;


    protected virtual void Start()
    {
        // Set critical dampening force to avoid oscillation.
        m_dampingForce = Mathf.Sqrt(4.0f * SnapSpringForce);
        
    }

    protected override void ResetPivots()
    {
        base.ResetPivots();
        m_contentPivot = ContentTransform.localPosition;
		if (cam != null) {
			camPivot = cam.transform.eulerAngles;
		}
    }

    
    protected override void ApplyPhysics()
    {
        //applyOverrunSpringForces();
      //  applyDrag();
    //    applyVelocity();
    }
    
    protected void applyDrag()
    {
        m_velocity -= m_velocity * Mathf.Max(0, Drag * Time.deltaTime);
    }

    
    protected void applyVelocity()
    {
        Vector3 currentPosition = ContentTransform.localPosition;
        currentPosition.y += m_velocity * Time.deltaTime;
        ContentTransform.localPosition = currentPosition;
    }
    
    /// <summary>
    /// Applies a spring force to velocity to return content to scroller bounds.
    /// </summary>
    protected void applyOverrunSpringForces()
    {
        float overrunDistance = calculateOverrunMagnitude() * InteractionScale;

        if (overrunDistance != 0.0f)
        {
            float springForce = calculate1DSpringForce(overrunDistance);
            m_velocity += springForce * Time.deltaTime;
        }
    }

    // offsetVector is the vector by which the object is offset from the goal
    protected float calculate1DSpringForce(float offsetVector)
    {
        float springForce = offsetVector * SnapSpringForce;
        float dampingForce = m_dampingForce * (m_velocity);
        return springForce - dampingForce;
    }

    // calc the amount and direction (if any) the content has overrun the scroll container.
    protected float calculateOverrunMagnitude()
    {
        float overrunDistance = 0.0f;

        // Put all positions in object space.
        Vector3 localContentTop = transform.InverseTransformPoint(ContentTopBound.position);
        Vector3 localContentBottom = transform.InverseTransformPoint(ContentBottomBound.position);
        Vector3 localContainerTop = transform.InverseTransformPoint(ContainerTopBound.position);
        Vector3 localContainerBottom = transform.InverseTransformPoint(ContainerBottomBound.position);

        if (localContentTop.y < localContainerTop.y)
        {
            overrunDistance = localContainerTop.y - localContentTop.y;
        }
        else if (localContentBottom.y > localContainerBottom.y)
        {
            overrunDistance = localContainerBottom.y - localContentBottom.y;
        }

        return overrunDistance;
    }

    protected override void ApplyInteractions()
    {
		if(cam != null){
	        Vector3 targetInteractorPositionChange = transform.parent.InverseTransformPoint(m_target.transform.position) - m_targetPivot;
	        targetInteractorPositionChange *= InteractionScale;	       
	        float tmp = targetInteractorPositionChange.x;
	        targetInteractorPositionChange.x = targetInteractorPositionChange.y;
	        targetInteractorPositionChange.y = -tmp;
	        targetInteractorPositionChange.z = 0.0f;	        
	        cam.transform.eulerAngles = camPivot + targetInteractorPositionChange;	        				       
		}
    }

    protected override void ApplyConstraints()
    {
        return;
    }

	public void SetCamera(Camera newCam){
		cam = newCam;
		if(cam != null)
			touchScreen.GetComponent<Renderer>().material.SetTexture("_MainTex", cam.targetTexture);
		else 
			touchScreen.GetComponent<Renderer>().material.SetTexture("_MainTex", black_material.GetTexture("_MainTex"));
	}

}
