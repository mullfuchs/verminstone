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

	// Use this for initialization
	void Start () {
		//grab all the carriers/miners
		NPCMiners = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		NPCCarriers = GameObject.FindGameObjectsWithTag ("Carrier");
		InitializeGatheringArea ();
		//send all miners to a standing position
		SendNPCsToGeneralAreaOfTarget(NPCMiners, gatheringAreaLocationObjects);
		//set em to go to the "bucket" and then to standing area
		SendNPCsToTargetWithFollowup(NPCCarriers, stoneBucketObject, gatheringAreaLocationObjects, NPCMiners.Length);
		//wait for weighing
		StartCoroutine(weighStoneSequence());
		//send them to a barrack

	}

	// Update is called once per frame
	void Update () {
		
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
			NPCGroup [i].GetComponent<AIStateMachine> ().SetNPCTarget (spots[i]);
		}
	}

	void SendNPCsToTargetWithFollowup(GameObject[] NPCGroup, GameObject target, GameObject[] spots, int IndexOffset){
		for (int i = 0; i < NPCGroup.Length; i++) {
			NPCGroup[i].GetComponent<AIStateMachine> ().SetNPCTarget (target);
			NPCGroup[i].GetComponent<AIStateMachine> ().SetFollowupTarget (spots [i + IndexOffset]);
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
		EventController.GetComponent<CampEventController>().SendNPCsToBarracks();
	}

}
