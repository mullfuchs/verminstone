using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaObjectTracker : MonoBehaviour {

    public int numberOfNPCsInRadius;

	// Use this for initialization
	void Start () {
        numberOfNPCsInRadius = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        numberOfNPCsInRadius++;
    }

    void OnTriggerExit(Collider other)
    {
        numberOfNPCsInRadius--;
    }

}
