using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudioSlow : MonoBehaviour {

	public Flying flying;
	private AudioSource windSlow;

	void Start()
	{
		windSlow = GetComponent<AudioSource>();
	}	
	// Update is called once per frame
	void Update () {
		float volume = Mathf.Clamp01(2 - flying.GetSpeed()/10);
		float heightMultiplier = Mathf.Clamp01(flying.GetAltitude()/30f);
		windSlow.volume = volume * heightMultiplier;
		Debug.Log(" slow: " + volume);
	}
}
