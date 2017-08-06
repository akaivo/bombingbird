using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingYawController : MonoBehaviour {

	public GameObject torso;
	public Flying flying;

	void Update () {
		transform.position = torso.transform.position;
		transform.rotation = Quaternion.LookRotation(flying.GetHeading(), Vector3.up);
	}
}
