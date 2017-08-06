using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlaySound : MonoBehaviour {

	public AudioClip[] clips;
	private int current = 0;
	List<AudioSource> playingSources = new List<AudioSource>();
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "shit")
        {
            playRandomSound();
        }
    }

    private void playRandomSound()
    {
        if(clips.Length > 0)
		{
			AudioSource newSource = gameObject.AddComponent<AudioSource>();
			newSource.clip = clips[current];
			newSource.loop = false;
			newSource.Play();
			newSource.maxDistance = 50f;
			newSource.spatialize = true;
			newSource.spatialBlend = .9f;
			playingSources.Add(newSource);
			current++;
			if(current > clips.Length - 1){
				current = 0;
			}
		}
    }

	void Update()
	{
		List<AudioSource> toBeRemoved = new List<AudioSource>();
		foreach (AudioSource source in playingSources)
		{
			if(!source.isPlaying)
			{
				toBeRemoved.Add(source);
			}
		}
		foreach (AudioSource deadSource in toBeRemoved)
		{
			playingSources.Remove(deadSource);
			Destroy(deadSource);
		}
	}
}
