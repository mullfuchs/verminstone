using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidPlayerCharachter : MonoBehaviour {

    private GameObject CharacterToAvoid;
    private Rigidbody thisRigidbody;
    private bool scriptEnabled = true;

    private float distanceToAvoidObject;
    private float forceDistance = 1.8f;
    private float forcePower = 10.0f;

	// Use this for initialization
	void Start () {
        CharacterToAvoid = GameObject.Find("Player");
        thisRigidbody = transform.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (scriptEnabled)
        {
            distanceToAvoidObject = Vector3.Distance(transform.position, CharacterToAvoid.transform.position);
            if(distanceToAvoidObject <= forceDistance)
            {
                thisRigidbody.AddRelativeForce((Vector3.back + CharacterToAvoid.transform.position) * -forcePower);
            }
        }
	}
}
