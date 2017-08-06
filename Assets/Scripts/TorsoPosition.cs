using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoPosition : MonoBehaviour
{

    public GameObject head;

    void Update()
    {
		transform.position = head.transform.position + Vector3.down * 0.3f;
    }
}
