using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudioFast : MonoBehaviour {

	public Flying flying;
	private AudioSource windFast;

	void Start()
	{
		windFast = GetComponent<AudioSource>();
	}	
	// Update is called once per frame
	void Update () {
		float volume = Mathf.Clamp01(-1 + flying.GetSpeed()/10);
		windFast.volume = volume;
		Debug.Log("fast: " + volume);
	}
}
