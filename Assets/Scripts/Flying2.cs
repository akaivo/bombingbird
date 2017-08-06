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

    public float flapStrengthWingsOpen = 0.05f;

    public float actualFlapStrength;

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

        get { return left.transform.position - right.transform.position; }

    }

    public Vector3 wingForward

	{

		get 

		{

			Vector3 leftForward = left.transform.forward + left.transform.right;

			Vector3 rightForward = right.transform.forward - right.transform.right;

			return Vector3.Lerp(leftForward, rightForward, 0.5f).normalized;

		}

	}

    private Vector3 direction;

    private float yawSensitivity = 0.003f;

    public float brakingConstant = 2f;
    public float diveConstant = 2f;

    //smoothing

    public int avgParam = 4;//seems to be good balance between smoothing and delay when using simple moving average;
    private Vector3 wingForwardSmoothed;
    private Queue wingForwards = new Queue(4);
    public GameObject torsoGameObject;

    void Start()

    {

        prevAngle = angle;

    }

    void Update()

    {

        //roll turning

        direction = Vector3.Cross(Vector3.up, leftToRight).normalized;

        Vector3 toTheRightParallelToGround = Vector3.ProjectOnPlane(leftToRight, Vector3.up);

        float roll = Vector3.Angle(leftToRight, toTheRightParallelToGround);

        float rollSign = (Vector3.Dot(direction, Vector3.Cross(leftToRight, toTheRightParallelToGround)) < 0) ? -1 : 1;

        float deltaYaw = roll * roll * speed * yawSensitivity * rollSign;

        transform.RotateAround(head.transform.position, Vector3.up, deltaYaw * Time.deltaTime);



        //flap and speed

        direction = Vector3.Cross(Vector3.up, leftToRight).normalized;

        float deltaAngle = Mathf.Abs(angle - prevAngle);

        if (prevAngle < angle)
        {//make upflap weaker

            deltaAngle *= 0.5f;

        }

        prevAngle = angle;

        crossSection = leftToRight.magnitude * 0.1f;

        actualFlapStrength = flapStrengthWingsOpen * leftToRight.magnitude * leftToRight.magnitude;

        drag = Mathf.Clamp(speed * speed * crossSection * dragConstant, 0.003f, 1000f);
        Vector3 upDown = Vector3.Project(wingForwardSmoothed, Vector3.up);
        direction += upDown;
        if(upDown.y > 0){
            drag += upDown.magnitude * upDown.magnitude * brakingConstant; 
        } else {
            speed += upDown.magnitude * diveConstant;
        }

        speed = Mathf.Clamp(speed - drag, 0f, maxSpeed);

        speed = Mathf.Clamp(speed + deltaAngle * actualFlapStrength, 0f, maxSpeed);
        

        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        wingForwards.Enqueue(wingForward);
        while (wingForwards.Count > avgParam) wingForwards.Dequeue();
        wingForwardSmoothed = GetMeanVector(wingForwards);

        torsoGameObject.transform.position = head.transform.position + Vector3.down*0.3f;
    }

    private Vector3 GetMeanVector(Queue positions)

    {

        if (positions.Count == 0)

            return Vector3.zero;

        float x = 0f;

        float y = 0f;

        float z = 0f;

        foreach (Vector3 pos in positions)

        {

            x += pos.x;

            y += pos.y;

            z += pos.z;

        }

        return new Vector3(x / positions.Count, y / positions.Count, z / positions.Count);

    }
}