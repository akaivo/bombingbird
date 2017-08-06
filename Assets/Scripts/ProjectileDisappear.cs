using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDisappear : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("TimeOutDestroy");
	}
	
	void OnCollisionEnter(Collision other)
	{
		Destroy(gameObject);
	}
	
	private IEnumerable TimeOutDestroy()
	{
		yield return new WaitForSeconds(10f);
		Destroy(gameObject);
	}
}
