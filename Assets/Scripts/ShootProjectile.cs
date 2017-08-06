using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : Photon.MonoBehaviour {

	public InputEvents inputEvents;
	public float minMass = 0.1f;
	public float maxMass = 10f;
	public float chargingSpeed = 1f;
	public float mass = 0f;
	public GameObject torso;
	public GameObject shitPrefab;

	private bool charging = false;
	private Vector3 previousPosition;
	private Vector3 initialVelocity;

	void OnEnable()
	{
		inputEvents.OnStartCharging += startCharging;
		inputEvents.OnRelease += release;
	}


    void OnDisable()
	{
		inputEvents.OnStartCharging -= startCharging;
		inputEvents.OnRelease -= release;
	}
	
	// Update is called once per frame
	void Update () {
		initialVelocity = (torso.transform.position - previousPosition) / Time.deltaTime;
		previousPosition = torso.transform.position;
		if(charging)
		{
			mass = Mathf.Clamp(mass + chargingSpeed * Time.deltaTime, minMass, maxMass);
		}
	}
	
    private void startCharging(object sender, EventArgs e)
    {
        mass = minMass;
		charging = true;
    }

	private void release(object sender, EventArgs e)
    {
		charging = false;
		photonView.RPC("makeShitHappen", PhotonTargets.All, torso.transform.position, initialVelocity, mass);
    }

	[PunRPC]
	public void makeShitHappen(Vector3 pos, Vector3 initialVelocity, float intialMass)
	{
		Rigidbody newShit = ((GameObject)Instantiate(shitPrefab, pos, Quaternion.identity)).GetComponent<Rigidbody>();
		newShit.velocity = initialVelocity;
		newShit.mass = intialMass;
		float radius = Mathf.Pow((3 * intialMass / (4 * Mathf.PI)), 1.0f / 3f);
		newShit.gameObject.transform.localScale = Vector3.one * radius;
		Debug.Log("Dropped a motherload of " + intialMass + " kg and velocity of " + initialVelocity);
	}
}
