using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController : MonoBehaviour {

	public bool canEndDay = true;

    public SpriteRenderer iconIndicator;

    public TextMesh actionDescriptionText;

    public Sprite EnterIcon;
    public Sprite GoUpIcon;
    public Sprite GoDownIcon;
    public Sprite FoodIcon;
    public Sprite SleepIcon;
    public Sprite Talkicon;
    public Sprite EscapeIcon;
    public Sprite EquipIcon;

    private bool isActionButtonPressed;

	private CampEventController CampEventControllerInstance;
	// Use this for initialization
	void Start () {
		CampEventControllerInstance = GameObject.Find ("CampEventController").GetComponent<CampEventController> ();
        actionDescriptionText.text = "";

    }
	
	// Update is called once per frame
	void Update () {
        //action button starts and stops action, freezes movement
        //cancel button remove action. 
        if (Input.GetButtonDown("Action"))
        {
            isActionButtonPressed = true;
        }

        if(Input.GetButtonUp("Action"))
        {
            isActionButtonPressed = false;
        }
	}

	void OnTriggerEnter(Collider Other){
		if (Other.tag == "MessHall") {
            //CampEventControllerInstance.StartMessHallSequence ();
            //why the fuck was this turned off? uhh 
            iconIndicator.sprite = FoodIcon;
            actionDescriptionText.text = "Eat";
        }
        if (Other.tag == "EquipArea"){
            iconIndicator.sprite = EquipIcon;
            actionDescriptionText.text = "Equip Team";
            //CampEventControllerInstance.StartEquipAreaSequence();
        }
		if (Other.tag == "CaveEntrance") {
            iconIndicator.sprite = EnterIcon;
            actionDescriptionText.text = "Enter Cave";
            //gameObject.GetComponent<NPCTeamHandler> ().rebuildNPCLists ();
            //CampEventControllerInstance.EnterCaveSequence ();
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
        if(Other.tag == "TunnelDigArea")
        {
            iconIndicator.sprite = EnterIcon;
            actionDescriptionText.text = "Dig Tunnel";
            //CampEventControllerInstance.StartTunnelDigSequence();
        }
        if(Other.tag == "bed")
        {
            iconIndicator.sprite = SleepIcon;
            actionDescriptionText.text = "Sleep";
        }

        if(Other.name == "PassageUp(Clone)")
        {
            iconIndicator.sprite = GoUpIcon;
            actionDescriptionText.text = "Ascend";
        }

        if(Other.name == "PassageDown(Clone)")
        {
            iconIndicator.sprite = GoDownIcon;
            actionDescriptionText.text = "Descend";
        }

        if(Other.tag == "NPCDialogTrigger")
        {
            ///hmmm I could put in a switch here so that the NPC is only able to be talked to once that day, checking the"has been talked to"
            ////becuse that would fix one or two problems, 
            if(gameObject.GetComponent<Yarn.Unity.Example.DialogTrigger>().canTalkToNPCs)
            {
                iconIndicator.sprite = Talkicon;
                string npcName = Other.GetComponentInParent<NPCstats>().NPCName;
                if (npcName != null && npcName != "")
                {
                    actionDescriptionText.text = "Talk To " + npcName;
                }
            }

        }
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "bed" && Input.GetKeyDown(KeyCode.Space) && canEndDay) {
			print ("Ending day");
			canEndDay = false;
			CampEventControllerInstance.EndDay ();
		}

        if (Input.GetButtonDown("Action"))
        {
            
            switch (other.tag)
            {
                case "CaveEntrance":
                    gameObject.GetComponent<NPCTeamHandler>().rebuildNPCLists();
                    CampEventControllerInstance.EnterCaveSequence();
                    break;
                case "MessHall":
                    CampEventControllerInstance.StartMessHallSequence();
                    break;
                case "EquipArea":
                    CampEventControllerInstance.StartEquipAreaSequence();
                    break;
                case "TunnelDigArea":
                    CampEventControllerInstance.StartTunnelDigSequence();
                    break;
                default:
                    break;
            }
           // isActionButtonPressed = false;
        }



    }


	void OnTriggerExit(Collider Other){
        //generally remove the icon.
        iconIndicator.sprite = null;

        actionDescriptionText.text = "";

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

        if (Other.tag == "TunnelDigArea")
        {
            CampEventControllerInstance.EndTunnelDigSequence();
        }
    }
}
