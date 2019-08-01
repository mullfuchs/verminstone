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
		if (Other.tag == "MessHall") {
			CampEventControllerInstance.StartMessHallSequence ();
		}
        if (Other.tag == "EquipArea"){
            CampEventControllerInstance.StartEquipAreaSequence();
        }
		if (Other.tag == "CaveEntrance") {
			gameObject.GetComponent<NPCTeamHandler> ().rebuildNPCLists ();
			CampEventControllerInstance.EnterCaveSequence ();
		}
		if (Other.tag == "CampArea") {
			//start the npc idle stuff
			CampEventControllerInstance.gameObject.GetComponent<NPCBedController>().AssignBeds();
			//start the end day timer in the campevent controller??? Sure???
		}
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "bed" && Input.GetKeyDown(KeyCode.Space)) {
			print ("Ending day");
			CampEventControllerInstance.EndDay ();
		}
	}


	void OnTriggerExit(Collider Other){
		if (Other.tag == "MessHall") {
			CampEventControllerInstance.EndMessHallSequence ();
		}
		if (Other.tag == "EquipArea") {
			CampEventControllerInstance.EndEquipAreaSequence ();
		}

        if (Other.tag == "CaveExit")
        {
			CampEventControllerInstance.refreshReferences ();

			//CampEventControllerInstance.ExitCaveSequence();
        }
    }
}
