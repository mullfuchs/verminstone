using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController : MonoBehaviour {


	private CampEventController CampEventControllerInstance;
	// Use this for initialization
	void Start () {
		CampEventControllerInstance = GameObject.Find ("CampEventController").GetComponent<CampEventController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider Other){
		if (Other.tag == "bed") {
			CampEventControllerInstance.EndDay ();
		}
		if (Other.tag == "MessHall") {
			CampEventControllerInstance.StartMessHallSequence ();
		}
	}
}
