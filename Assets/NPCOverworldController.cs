using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCOverworldController : MonoBehaviour {

	//this controls the behavior of NPCs above ground. 
	//I'm not sure how this will work yet but basiclly we got several use cases to consider
	/*
	 * 1 exiting the cave. doing stuff like putting vstone away if they have a bag, putting tools away. discarding tools if they're
	 * totally done and don't wanna go back in the cave. rebellion at the cave entrance. etc
	 * 
	 * 2 ambling around bunks, going to bed, eating. stuff like that. giving the oppertunity for the player to talk to them
	 * and for them to come up to the player. 
	 * 
	 * 3 triggering story sequences with the npc, things like leading the player to a secret area. escape sequences. rebellion. etc
	 * 
	 * 4 morning sequence.
	 * 
	 * etc
	 * 
	 * but for now I just need them to go to a random area so I can talk to them, and then eat, then bed. and then when you wake up they go eat
	 */

	public bool idling = false;

	float idleTime = 0.0f;
	float idlePeriod = 15.0f;
	float idleCounter = 1;

	public bool isEscaping;
	GameObject campEscapeObject;

	GameObject[] npcIdleTargets;
	int idleTargetIndex = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (idling)
        {
            //start a countdown, if it's below a threshold, go to the next target in idle targets;
            idleTime -= Time.deltaTime;
            if (idleTime <= 0.0f)
            {
                idleCounter -= 1;
                if (idleCounter <= 0)
                {
					if (!isEscaping) {
						GoToBed ();
					} else {
						
					}
                    idling = false;
                }
                else
                {
                    if (idleTargetIndex > npcIdleTargets.Length)
                    {
                        idleTargetIndex = 0;
                    }
                    else
                    {
                        idleTargetIndex++;
                    }
                    gameObject.GetComponent<AIStateMachine>().SendNPCToObject(npcIdleTargets[idleTargetIndex]);
                    idleTime = idlePeriod;
                }
            }
        }


	}

	void GoToBed(){
        //find a bedn
       // print("npc going to bed");
		//idling = false;
		if (gameObject.GetComponent<NPCstats> ().bedIndex != null) {
            GameObject bed = GameObject.Find("CampEventController").GetComponent<NPCBedController>().npcBeds[ gameObject.GetComponent<NPCstats> ().bedIndex ];
			gameObject.GetComponent<AIStateMachine> ().SendNPCToObject (bed);
			//God this sucks, but what can ya do lol
		}
	}

	void GoToEscapeObject(){
		if (campEscapeObject != null) {
			gameObject.GetComponent<AIStateMachine> ().SendNPCToObject (campEscapeObject);
		}
	}

	void DoIdleRoutine(){
		idling = true;
		idleTime = idlePeriod;
		npcIdleTargets = buildIdleTargetList ();
		gameObject.GetComponent<AIStateMachine> ().SendNPCToObject (npcIdleTargets[0]);
	}

	GameObject[] buildIdleTargetList(){
		GameObject[] idleTargets;

		idleTargets = GameObject.FindGameObjectsWithTag ("IdleLocation");
		shuffleArray (idleTargets);
        
		if (isEscaping) {
			campEscapeObject = GameObject.Find ("CampAreaSecretEscape");
		}

		return idleTargets;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "CampArea" && GameObject.Find("CampEventController").GetComponent<CampNarrativeController>().timeOfDay == CampNarrativeController.timePeriod.Evening) {
			//print ("npc doing idle routine");
			DoIdleRoutine ();
		}



	}

	void shuffleArray(GameObject[] targets)
	{
		// Knuth shuffle algorithm :: courtesy of Wikipedia :)
		for (int t = 0; t < targets.Length; t++ )
		{
			GameObject tmp = targets[t];
			int r = Random.Range(t, targets.Length);
			targets[t] = targets[r];
			targets[r] = tmp;
		}

	}
}
