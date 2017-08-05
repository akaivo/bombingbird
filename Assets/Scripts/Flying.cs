using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Flying : MonoBehaviour
{

    public GameObject head;
    public GameObject left;
    public GameObject right;
    private float speed;
    public GameObject[] debug;
    private Vector3 direction;
    // //private EVRButtonId trigger = EVRButtonId.k_EButton_SteamVR_Trigger;
    // private SteamVR_Controller.Device left { get { return SteamVR_Controller.Input((int)leftC.GetComponent<SteamVR_TrackedObject>().index); } }
    // private SteamVR_Controller.Device right { get { return SteamVR_Controller.Input((int)rightC GetComponent<SteamVR_TrackedObject>().index); } }
    private Vector3 vectorFromLeftToRight { get { return rightSmoothedPosition - leftSmoothedPosition; } }
    private Vector3 averageForward { get { return Vector3.Lerp(rightSmoothedForward, leftSmoothedForward, 0.5f); } }
    private Vector3 torsoPosition
    {
        get
        {
			Vector3 Rr = rightSmoothedPosition - headSmoothedPosition;
			Vector3 Lr = leftSmoothedPosition - headSmoothedPosition;
            Vector3 DownRel = 0.3f * Vector3.Lerp(Rr, Lr, 0.5f).normalized;
			return headSmoothedPosition + DownRel;
		}
    }
    //smoothing
    public int avgParam = 4;//seems to be good balance between smoothing and delay when using simple moving average;
    private Vector3 headSmoothedPosition;
    private Vector3 rightSmoothedPosition;
    private Vector3 leftSmoothedPosition;
    private Vector3 rightSmoothedForward;
    private Vector3 leftSmoothedForward;
	private Queue headPositions = new Queue(4);
    private Queue rightPositions = new Queue(4);
    private Queue leftPositions = new Queue(4);

    private Queue rightForwards = new Queue(4);
    private Queue leftForwards = new Queue(4);

    // Update is called once per frame
    void Update()
    {
		headPositions.Enqueue(head.transform.position);
        while (headPositions.Count > avgParam) headPositions.Dequeue();
        headSmoothedPosition = GetMeanVector(headPositions);

        rightPositions.Enqueue(right.transform.position);
        leftPositions.Enqueue(left.transform.position);
        while (rightPositions.Count > avgParam) rightPositions.Dequeue();
        while (leftPositions.Count > avgParam) leftPositions.Dequeue();
        rightSmoothedPosition = GetMeanVector(rightPositions);
        leftSmoothedPosition = GetMeanVector(leftPositions);

        rightForwards.Enqueue(right.transform.forward);
        leftForwards.Enqueue(left.transform.forward);
        while (rightForwards.Count > avgParam) rightForwards.Dequeue();
        while (leftForwards.Count > avgParam) leftForwards.Dequeue();
        rightSmoothedForward = GetMeanVector(rightForwards);
        leftSmoothedForward = GetMeanVector(leftForwards);
        debug[0].transform.position = torsoPosition;


        direction = Vector3.Cross(Vector3.up, vectorFromLeftToRight).normalized + averageForward;
        transform.position += direction * speed * Time.deltaTime;
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
