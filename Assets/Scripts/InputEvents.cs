using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputEvents : MonoBehaviour {

	public event EventHandler OnStartCharging;
	public event EventHandler OnRelease;

	public GameObject rightGO;
    public GameObject leftGO;
    private SteamVR_Controller.Device right { get { return SteamVR_Controller.Input((int)rightGO.GetComponent<SteamVR_TrackedObject>().index); } }
    private SteamVR_Controller.Device left { get { return SteamVR_Controller.Input((int)leftGO.GetComponent<SteamVR_TrackedObject>().index); } }
    private EVRButtonId trigger = EVRButtonId.k_EButton_SteamVR_Trigger;
    private EVRButtonId grip = EVRButtonId.k_EButton_Grip;
	
	// Update is called once per frame
	void Update ()
	{
		if(right.GetTouchDown(trigger))	
		{
			OnStartCharging(this, EventArgs.Empty);
		}
		if(right.GetTouchUp(trigger))	
		{
			OnRelease(this, EventArgs.Empty);
		}
	}
}
