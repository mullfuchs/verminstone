using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObjectsWithinRadius : MonoBehaviour {

    private health PlayerHealth;

	// Use this for initialization
	void Start () {
        PlayerHealth = GameObject.Find("Player").GetComponent<health>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        health objectHealth = other.gameObject.GetComponent<health>();
        if(other.tag == "WorkerNPC" && objectHealth != null)
        {
            if (objectHealth.AddHealth(1.0f))
            {
                PlayerHealth.AddDamage(0.3f);
            }
        }

    }
}
