using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying2 : MonoBehaviour
{
    public GameObject head;
    public GameObject left;
    public GameObject right;
    public float speed = 0f;
    public float maxSpeed = 20f;
    public float crossSection = 0.1f;
    public float dragConstant = 1f;
    public float drag;
    public float flapStrengthWingsOpen = 0.03f;
    public float actualFlapStrength;
	public float upFlapStrength = 0.2f;
    private float prevAngle;
    public float angle
    {
        get
        {
            Vector3 Rr = right.transform.position - head.transform.position;
            Vector3 Lr = left.transform.position - head.transform.position;
            return Vector3.Angle(Rr, Lr);
        }
    }
    public Vector3 leftToRight
    {
        get { return right.transform.position - left.transform.position; }
    }

	public Vector3 wingForward
	{
		get 
		{
			Vector3 leftForward = left.transform.forward - left.transform.right;
			Vector3 rightForward = right.transform.forward - right.transform.right;
			return Vector3.Lerp(leftForward, rightForward, 0.5f).normalized;
		}
	}

    private Vector3 direction;
    private float yawSensitivity = 0.003f;

    void Start()
    {
        prevAngle = angle;
    }
    void Update()
    {
        //roll turning
		direction = Vector3.Cross(Vector3.up, leftToRight).normalized;
        Vector3 toTheRight = Vector3.ProjectOnPlane(leftToRight, Vector3.up);
		float roll = Vector3.Angle(leftToRight, toTheRight);
        float rollSign = (Vector3.Dot(direction, Vector3.Cross(leftToRight, toTheRight)) < 0) ? -1 : 1;
        float deltaYaw = roll * roll * speed * yawSensitivity * rollSign;
        transform.RotateAround(head.transform.position, Vector3.up, deltaYaw * Time.deltaTime);

		//flap and speed
        direction = Vector3.Cross(Vector3.up, leftToRight).normalized;
        float deltaAngle = Mathf.Abs(angle - prevAngle);
		if(prevAngle < angle) {//make upflap weaker
			deltaAngle *= upFlapStrength;
		}
        prevAngle = angle;
        crossSection = leftToRight.magnitude * 0.1f;
        actualFlapStrength = flapStrengthWingsOpen * leftToRight.magnitude * leftToRight.magnitude;
        drag = Mathf.Clamp(speed * speed * crossSection * dragConstant, 1f, 1000f);
        speed = Mathf.Clamp(speed - drag * Time.deltaTime, 0f, maxSpeed);
        speed = Mathf.Clamp(speed + deltaAngle * actualFlapStrength, 0f, maxSpeed);

		//pitch and dive
		
        
		
		
		transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
