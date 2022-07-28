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
    public bool dialogOpened;

	private CampEventController CampEventControllerInstance;

    public GameObject npcIMightTalkTo;
	// Use this for initialization
	void Start () {
		CampEventControllerInstance = GameObject.Find ("CampEventController").GetComponent<CampEventController> ();
        actionDescriptionText.text = "";
        dialogOpened = false;
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

        if(Input.GetButtonDown("Pause") && dialogOpened == false)
        {
            GameObject.Find("Canvas").GetComponent<PauseMenuEnabler>().EnablePauseMenu();
            dialogOpened = true;
        }
	}

	void OnTriggerEnter(Collider Other){
		if (Other.tag == "MessHall") {
            //CampEventControllerInstance.StartMessHallSequence ();
            //why the fuck was this turned off? uhh 
            iconIndicator.sprite = FoodIcon;
            actionDescriptionText.text = "Eat";
        }
        if (Other.tag == "EquipArea" && !isEvening()){
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
            //CampEventControllerInstance.MakeNonWorkernPCsIdle();

            //start the end day timer in the campevent controller??? Sure???
			if (CampEventControllerInstance.GetComponent<CampNarrativeController> ().timeOfDay == CampNarrativeController.timePeriod.Evening) {
				//gameObject.GetComponent<Yarn.Unity.Example.DialogTrigger> ().canTalkToNPCs = true;
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
        if(Other.tag == "bed" && isEvening())
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
            if(gameObject.GetComponent<Yarn.Unity.Example.DialogTrigger>().canTalkToNPCs && Other.gameObject.GetComponentInParent<Yarn.Unity.Example.NPC>().canTalkTo)
            {
                npcIMightTalkTo = Other.gameObject;
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
		if (other.tag == "bed" && Input.GetButtonDown("Action") && canEndDay && isEvening()) {
			print ("Ending day");
			canEndDay = false;
			CampEventControllerInstance.EndDay ();
		}

        if (Input.GetButtonDown("Action") && !dialogOpened)
        {
            
            switch (other.tag)
            {
                case "CaveEntrance":
                    gameObject.GetComponent<NPCTeamHandler>().rebuildNPCLists();
                    CampEventControllerInstance.EnterCaveSequence();
                    break;
                case "MessHall":
                    CampEventControllerInstance.StartMessHallSequence();
                    dialogOpened = true;
                    SetPlayerMovement(false);
                    break;
                case "EquipArea":
                    if (GameObject.Find("CampEventController").GetComponent<CampNarrativeController>().timeOfDay == CampNarrativeController.timePeriod.Morning)
                    {
                        CampEventControllerInstance.StartEquipAreaSequence();
                        dialogOpened = true;
                        SetPlayerMovement(false);
                    }
                    break;
                case "TunnelDigArea":
                    CampEventControllerInstance.StartTunnelDigSequence();
                    dialogOpened = true;
                    SetPlayerMovement(false);
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
            dialogOpened = false;
		}
		if (Other.tag == "EquipArea") {
			CampEventControllerInstance.EndEquipAreaSequence ();
            dialogOpened = false;
		}

        if (Other.tag == "CaveExit")
        {
			CampEventControllerInstance.refreshReferences ();
            dialogOpened = false;
            //CampEventControllerInstance.ExitCaveSequence();
        }

        if (Other.tag == "TunnelDigArea")
        {
            CampEventControllerInstance.EndTunnelDigSequence();
            dialogOpened = false;
        }

        if(Other.tag == "NPCDialogTrigger")
        {
            npcIMightTalkTo = null;
        }
    }

    public void SetPlayerMovement(bool canMove)
    {
        gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().can_move = canMove;
    }

    public bool isEvening()
    {
        return CampEventControllerInstance.GetComponent<CampNarrativeController>().timeOfDay == CampNarrativeController.timePeriod.Evening;
    }
}
