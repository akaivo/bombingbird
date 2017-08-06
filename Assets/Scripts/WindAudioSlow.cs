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
		windSlow.volume = volume;
		Debug.Log(" slow: " + volume);
	}
}
