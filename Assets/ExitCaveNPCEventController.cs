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
		NPCMiners = teamHandler.GetCurrentMiners().ToArray();
		NPCCarriers = teamHandler.GetCurrentCarriers().ToArray();
		InitializeGatheringArea ();
        VStoneEcoInstance = GameObject.Find("CampEventController").GetComponent<VStoneEconomyObject>();
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void doCaveExitEvent()
    {
        SendNPCsToGeneralAreaOfTarget(NPCMiners, gatheringAreaLocationObjects);
        //set em to go to the "bucket" and then to standing area
        SendNPCsToTargetWithFollowup(NPCCarriers, stoneBucketObject, gatheringAreaLocationObjects, NPCMiners.Length);
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
		for (int i = 0; i < NPCGroup.Length; i++) {
			NPCGroup [i].GetComponent<AIStateMachine> ().AddTargetForNPC (spots[i]);
		}
	}

	void SendNPCsToTargetWithFollowup(GameObject[] NPCGroup, GameObject target, GameObject[] spots, int IndexOffset){
		for (int i = 0; i < NPCGroup.Length; i++) {
			NPCGroup[i].GetComponent<AIStateMachine> ().SendNPCToObject (target);
            NPCGroup[i].GetComponent<AIStateMachine> ().AddTargetForNPC (spots [i + IndexOffset]);
		}
	}

	IEnumerator weighStoneSequence(){
        EventController.GetComponent<CampEventController>().currentStagingArea = gatheringAreaObject;
        int carrierCount = NPCCarriers.Length;
		//have all the carriers dropped stone?
		while (stoneBucketObject.GetComponent<RockBucketController> ().getNumberOfVisitedCarriers() < carrierCount) {
			yield return null;
		}
		yield return new WaitForSeconds (2.0f);
		GameObject foreman = GameObject.FindWithTag ("Foreman");
		GameObject.Find ("MultipurposeCameraRig").GetComponent<ZoomNFocus> ().focusOnNPC (foreman.transform);
        //start foreman dialog
        FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(foreman.GetComponent<Yarn.Unity.Example.NPC>().talkToNode);
        //wait until dialog finished
        yield return new WaitForSeconds (4.0f);
		GameObject.Find ("MultipurposeCameraRig").GetComponent<ZoomNFocus> ().reset ();
        print("total vstone collected this run: " + VStoneEcoInstance.getDailyTotal());
		EventController.GetComponent<CampEventController>().SendNPCsToBarracks();
	}

}
