using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingTracker : Photon.MonoBehaviour {

	public GameObject localWing;
	
	// Update is called once per frame
	void Update () {
		if(photonView.isMine) {
			GetComponent<Renderer>().enabled = false;
		} else {
			return;
		}
		transform.position = localWing.transform.position;
		transform.rotation = localWing.transform.rotation;
	}
}
