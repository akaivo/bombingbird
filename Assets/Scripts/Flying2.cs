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
		get { return right.transform.position - left.transform.position;}
	}
	
	private Vector3 direction;
	

	void Start()
	{
		prevAngle = angle;
	}
    void Update()
    {
		float deltaAngle = Mathf.Abs(angle - prevAngle);
		prevAngle = angle;
		crossSection = leftToRight.magnitude * 0.1f;
		actualFlapStrength = flapStrengthWingsOpen * leftToRight.magnitude * leftToRight.magnitude;
		drag = Mathf.Clamp(speed * speed * crossSection * dragConstant, 1f, 1000f);
		speed = Mathf.Clamp(speed - drag * Time.deltaTime, 0f, maxSpeed);
		speed = Mathf.Clamp(speed + deltaAngle * actualFlapStrength, 0f, maxSpeed);

		direction = Vector3.Cross(Vector3.up, right.transform.position - left.transform.position).normalized;

		transform.Translate(direction * speed * Time.deltaTime);
    }
}
