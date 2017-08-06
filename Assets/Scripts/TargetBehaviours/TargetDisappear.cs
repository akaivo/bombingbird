using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDisappear : MonoBehaviour {

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "shit")
		{
			Destroy(gameObject);
		}
	}
}
