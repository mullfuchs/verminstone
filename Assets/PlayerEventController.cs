using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController : MonoBehaviour {

	public bool canEndDay = true;

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
			if (CampEventControllerInstance.GetComponent<CampNarrativeController> ().timeOfDay == CampNarrativeController.timePeriod.Evening) {
				gameObject.GetComponent<Yarn.Unity.Example.DialogTrigger> ().canTalkToNPCs = true;
                CampEventControllerInstance.NPCDialogEnabled(true);
            }

			if (CampEventControllerInstance.GetComponent<CampNarrativeController> ().timeOfDay == CampNarrativeController.timePeriod.Morning) {
				gameObject.GetComponent<Yarn.Unity.Example.DialogTrigger> ().canTalkToNPCs = false;
                CampEventControllerInstance.NPCDialogEnabled(false);
            }

		}

	}

	void OnTriggerStay(Collider other){
		if (other.tag == "bed" && Input.GetKeyDown(KeyCode.Space) && canEndDay) {
			print ("Ending day");
			canEndDay = false;
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
