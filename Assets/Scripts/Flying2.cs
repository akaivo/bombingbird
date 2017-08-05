using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying2 : MonoBehaviour
{
    public GameObject head;
    public GameObject left;
    public GameObject right;
    public Vector3 speed;
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

    private Vector3 yawDirection;
    private float yawSensitivity = 0.003f;
    public float diveAccel = 1;

    void Start()
    {
        prevAngle = angle;
    }
    void Update()
    {
        //roll turning
		yawDirection = Vector3.Cross(Vector3.up, leftToRight).normalized;
        Vector3 toTheRight = Vector3.ProjectOnPlane(leftToRight, Vector3.up);
		float roll = Vector3.Angle(leftToRight, toTheRight);
        float rollSign = (Vector3.Dot(yawDirection, Vector3.Cross(leftToRight, toTheRight)) < 0) ? -1 : 1;
        float deltaYaw = roll * roll * speed.magnitude * yawSensitivity * rollSign;
        transform.RotateAround(head.transform.position, Vector3.up, deltaYaw * Time.deltaTime);
        yawDirection = Vector3.Cross(Vector3.up, leftToRight).normalized;//calculate again after rotating
        
        //dive
		toTheRight = Vector3.ProjectOnPlane(leftToRight, Vector3.up);
        Vector3 pitchDirection = Vector3.ProjectOnPlane(wingForward, toTheRight).normalized;
        float diveAngle = Vector3.Angle(Vector3.down, pitchDirection);
        if(diveAngle < 90f){
            float diveMultiplier = Mathf.Cos(diveAngle * Mathf.PI / 180);
            Debug.Log(Mathf.Cos(diveAngle * Mathf.PI / 180));
            speed += Vector3.down * diveMultiplier * diveAccel * Time.deltaTime;
            speed += yawDirection * -speed.y * Time.deltaTime;
        } else {
            diveAngle -= 90;
            float riseMultiplier = Mathf.Sin(diveAngle * Mathf.PI / 180);
            speed += Vector3.up * riseMultiplier * speed.magnitude * Time.deltaTime;
        }


		//flap and speed
        float deltaAngle = Mathf.Abs(angle - prevAngle);
		if(prevAngle < angle) {//make upflap weaker
			deltaAngle *= upFlapStrength;
		}
        prevAngle = angle;
        actualFlapStrength = flapStrengthWingsOpen * leftToRight.magnitude * leftToRight.magnitude;
        
        speed = Vector3.ClampMagnitude(speed + yawDirection * deltaAngle * actualFlapStrength, maxSpeed);//flapping

        crossSection = leftToRight.magnitude * 1f; //wingspan open or closed
		float dragAngle = Vector3.Angle(speed, pitchDirection);
        if(dragAngle < 90f){//
            Debug.Log(Mathf.Sin(dragAngle * Mathf.PI / 180));
            crossSection *= Mathf.Sin(dragAngle * Mathf.PI / 180);
        }

        drag = Mathf.Clamp(speed.magnitude * speed.magnitude * crossSection * dragConstant, 1f, 1000f);
        speed = Vector3.ClampMagnitude(speed - speed.normalized * drag * Time.deltaTime, maxSpeed);
		
		
		transform.Translate(speed * Time.deltaTime, Space.World);
    }
}
