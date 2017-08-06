using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingController : MonoBehaviour {

	public GameObject torso;
	public GameObject left;
	public GameObject right;

	public float x,y,z;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = torso.transform.position;
		Quaternion leftRotation = Quaternion.Euler(x,y,z);
		transform.rotation = leftRotation;
	}
}
