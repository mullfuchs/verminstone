using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCaveNPCEventController : MonoBehaviour {

	public GameObject EventController;

	public GameObject gatheringAreaObject;
	public GameObject stoneBucketObject;

	public GameObject[] gatheringAreaLocationObjects;

	public GameObject[] NPCMiners = new GameObject[5];
	public GameObject[] NPCCarriers = new GameObject[5];

    private bool isWeighingdialogFinished = false;

    private NPCTeamHandler teamHandler;

    private VStoneEconomyObject VStoneEcoInstance;

    private bool hasExitDialogCompleted = false;

	// Use this for initialization
	void Start () {
        teamHandler = GameObject.Find("Player").GetComponent<NPCTeamHandler>();
		//NPCMiners = teamHandler.GetCurrentMiners().ToArray();
		//NPCCarriers = teamHandler.GetCurrentCarriers().ToArray();
		//InitializeGatheringArea ();
        VStoneEcoInstance = GameObject.Find("CampEventController").GetComponent<VStoneEconomyObject>();
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void doCaveExitEvent()
    {
		teamHandler = GameObject.Find("Player").GetComponent<NPCTeamHandler>();

        //teamHandler.resetNPCTargets ();
        //teamHandler.RefreshNPCMinerList();
        NPCMiners = teamHandler.GetCurrentMiners().ToArray();
        NPCCarriers = teamHandler.GetCurrentCarriers().ToArray();

		//debug get stones from rock carriers
		//debugGetStonesFromCarriers(NPCCarriers);


        //wait for weighing
        StartCoroutine(weighStoneSequence());
        //send them to a barrack
    }

    void InitializeGatheringArea(){
		int size = NPCMiners.Length + NPCCarriers.Length;
		gatheringAreaLocationObjects = new GameObject[size];
		for (int i = 0; i < gatheringAreaLocationObjects.Length; i++) {
			Vector2 randomSpot = Random.insideUnitCircle * 5;
			GameObject randomSpotObject = new GameObject ();
			randomSpotObject.transform.position = new Vector3 (randomSpot.x + gatheringAreaObject.transform.position.x, gatheringAreaObject.transform.position.y, randomSpot.y + gatheringAreaObject.transform.position.z);
			gatheringAreaLocationObjects [i] = randomSpotObject;
		}
	}

	void SendNPCsToGeneralAreaOfTarget(GameObject[] NPCGroup, GameObject[] spots){
		print ("sending npcs to target maybe");
		for (int i = 0; i < NPCGroup.Length; i++) {
            NPCGroup [i].GetComponent<AIStateMachine> ().SendNPCToObject (spots[i]);
		}
	}

	void SendNPCsToTargetWithFollowup(GameObject[] NPCGroup, GameObject target, GameObject[] spots, int IndexOffset){
		print ("sending carriers to target with followup");
		for (int i = 0; i < NPCGroup.Length; i++) {
            NPCGroup[i].GetComponent<AIStateMachine> ().SendNPCToObject (target);
            NPCGroup[i].GetComponent<AIStateMachine> ().AddTargetForNPC (spots [i + IndexOffset]);
		}
	}

	IEnumerator weighStoneSequence(){

		yield return new WaitForSeconds (2.0f);

        int carrierCount = NPCCarriers.Length;



		for(int i = 0; i < NPCCarriers.Length; i++){
            print("sending npc to rock bucket");
            while (NPCCarriers[i].GetComponent<UnityEngine.AI.NavMeshAgent>().isOnNavMesh == false)
            {
                print("npc not on nav mesh yet");
                yield return new WaitForSeconds(0.3f);
                //wait for thing to get on mesh before continuing
            }    

			NPCCarriers [i].GetComponent<AIStateMachine> ().SendNPCToObject (stoneBucketObject); 
		}

		//have all the carriers dropped stone?
		while (stoneBucketObject.GetComponent<RockBucketController> ().getNumberOfVisitedCarriers() < carrierCount) {
			yield return null;
		}
		yield return new WaitForSeconds (5.0f);

        teamHandler.emptyVstoneCollected();

        print("total vstone collected this run: " + VStoneEcoInstance.getDailyTotal());
		print ("total vstone collected during this save: " + VStoneEcoInstance.getTotalCollected ());

		if (VStoneEcoInstance.meetsDailyQuota (VStoneEcoInstance.getDailyTotal ())) {
            //door lowring thing goes here

            //pop dialog for success, or failure
            //populateDialogPortraits();
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("Overseer.Exit.Success1");
            //get overseer dialog portraits from keynpcs from CampNarrativeControllerObject
            Sprite[] OverseerPortraits = GameObject.Find("CampEventController").GetComponent<CampNarrativeController>().getPotraitsForKeyNPC("Overseer");
            FindObjectOfType<DialogPortraitController>().populateDialogPortraits(GameObject.Find("Player").GetComponent<NPCstats>().DialogPortraits, OverseerPortraits);

            while (hasExitDialogCompleted != true)
            {
                yield return null;
            }
            hasExitDialogCompleted = false;

			GameObject.Find("CaveExitDoor").GetComponent<DoorController>().OpenDoor();
			GameObject[] npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");

            for (int i = 0; i < npcs.Length; i++) {
                //since we're loading into the scene and dialog is now "new" we have to reload each npc's scripts into it's registry, or whatever
                npcs[i].GetComponent<Yarn.Unity.Example.NPC>().LoadNPCScript();
                //STUPID HACK, turning off and on navmesh agent, because one NPC will just not path for whatever reason
                npcs[i].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                npcs[i].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;

                npcs [i].GetComponent<AIStateMachine> ().SendNPCToObject ( GameObject.Find("EquipmentReturn") );
			}


            GameObject.Find("CampEventController").GetComponent<GameSaveController>().LoadQuestObjects();

            //EventController.GetComponent<CampEventController>().SendNPCsToBarracks();
        } else {
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("Overseer.Exit.Failure1");

            while (hasExitDialogCompleted != true)
            {
                yield return null;
            }
            hasExitDialogCompleted = false;

            print ("go back 2 the caves");
			EventController.GetComponent<CampEventController> ().caveEntrance.GetComponent<CaveEntrance> ().LoadLevelOnEnter = true;
			// caveEntrance.SetActive (true);
			VStoneEcoInstance.resetDailyTotal ();

		}

	}

	void debugGetStonesFromCarriers(GameObject[] NPCCarrierArray){
		print ("DEBUG GET STONES, FUCK");
		for (int i = 0; i < NPCCarrierArray.Length; i++) {
			VStoneEcoInstance.AddVStoneToDailyTotal( NPCCarrierArray [i].GetComponent<AIStateMachine> ().GetVerminStoneAmount ());
			print("DEBUG total vstone collected this run: " + VStoneEcoInstance.getDailyTotal());
		}

	}

    [Yarn.Unity.YarnCommand("completeCaveExitDialog")]
    public void completeExitDialog()
    {
        print("finishing exit dialog");
        hasExitDialogCompleted = true;
    }
}
