using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingRollController : MonoBehaviour {
	public GameObject torso;
	public GameObject leftHand;
	public GameObject rightHand;
	public GameObject leftWing;
	public GameObject rightWing;
	void Update () {
		Vector3 torsoToLeftHand = leftHand.transform.position - torso.transform.position;
		Vector3 torsoToRightHand = rightHand.transform.position - torso.transform.position;
		Vector3 roLParallel = Vector3.ProjectOnPlane(torsoToLeftHand, Vector3.up);
		Vector3 roRParallel = Vector3.ProjectOnPlane(torsoToRightHand, Vector3.up);
		float rollAngleLeft = Vector3.Angle(roLParallel, torsoToLeftHand);
		float rollAngleRight = Vector3.Angle(roRParallel, torsoToRightHand);
		if(torsoToLeftHand.y > 0) rollAngleLeft *= -1;
		if(torsoToRightHand.y < 0) rollAngleRight *= -1;
		leftWing.transform.localRotation = Quaternion.AngleAxis(rollAngleLeft, Vector3.forward);
		rightWing.transform.localRotation = Quaternion.AngleAxis(rollAngleRight, Vector3.forward);
		rightWing.transform.localRotation *= Quaternion.AngleAxis(180, Vector3.forward);
	}
}
