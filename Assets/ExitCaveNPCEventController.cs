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
			EventController.GetComponent<CampEventController>().SendNPCsToBarracks();
		} else {
			print ("go back 2 the caves");
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
}
