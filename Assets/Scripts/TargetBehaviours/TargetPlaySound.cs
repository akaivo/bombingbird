using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlaySound : MonoBehaviour {

	public AudioClip[] clips;
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
			int index = Random.Range(0,clips.Length);
			newSource.clip = clips[index];
			if(index == 0) newSource.volume = 0.3f;
			newSource.loop = false;
			newSource.Play();
			newSource.maxDistance = 100f;
			newSource.spatialize = true;
			newSource.spatialBlend = .7f;
			playingSources.Add(newSource);
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
