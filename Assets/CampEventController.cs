using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CampEventController : MonoBehaviour {

	public bool ForceDayToEndByPressingY = true;

	public GameObject Barracks;
	public GameObject caveStagingArea;

    public GameObject currentStagingArea;

	//public GameObject[] gatheringAreaLocationObjects;

	public GameObject[] NPCMiners = new GameObject[5];
	public GameObject[] NPCCarriers = new GameObject[5];

	private GameObject[] AllNPCs;

	public GameObject Sun;
	private float DayCycleClockTime;
	// Use this for initialization
	void Start () {
		
		NPCMiners = GameObject.FindGameObjectsWithTag ("Miner");
		NPCCarriers = GameObject.FindGameObjectsWithTag ("Carrier");
		AllNPCs = new GameObject[NPCMiners.Length + NPCCarriers.Length];
		NPCMiners.CopyTo (AllNPCs, 0);
		NPCCarriers.CopyTo (AllNPCs, NPCMiners.Length);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Y)){
			SendNPCsToStagingArea ();
		}
	}

	public void SendNPCsToBarracks(){
        CleanUpGatheringArea(currentStagingArea);
		SendNPCsToArea (AllNPCs, Barracks);
        currentStagingArea = Barracks;
	}

	public void SendNPCsToStagingArea(){
        CleanUpGatheringArea(currentStagingArea);
		SendNPCsToArea (AllNPCs, caveStagingArea);
        currentStagingArea = caveStagingArea;
	}

	public void EndDay(){
		StartCoroutine (EndDayCycle ());
	}

	void SendNPCsToArea(GameObject[] NPCGroup, GameObject target){
		GameObject[] spots = InitializeGatheringArea (target);
		for (int i = 0; i < NPCGroup.Length; i++) {
				NPCGroup [i].GetComponent<AIStateMachine> ().SetNPCTarget (spots[i]);
		}
	}

    void CleanUpGatheringArea(GameObject GatheringAreaObject)
    {
        foreach (Transform child in GatheringAreaObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    GameObject[] InitializeGatheringArea(GameObject gatheringAreaObject){
		GameObject[] gatheringAreaLocationObjects;
		int size = NPCMiners.Length + NPCCarriers.Length;
		gatheringAreaLocationObjects = new GameObject[size];
		for (int i = 0; i < gatheringAreaLocationObjects.Length; i++) {
			Vector2 randomSpot = Random.insideUnitCircle * 5;
			GameObject randomSpotObject = new GameObject ();
			randomSpotObject.transform.position = new Vector3 (randomSpot.x + gatheringAreaObject.transform.position.x, gatheringAreaObject.transform.position.y, randomSpot.y + gatheringAreaObject.transform.position.z);
			gatheringAreaLocationObjects [i] = randomSpotObject;
		}
		return gatheringAreaLocationObjects;
	}

	IEnumerator AdvanceDayCycle(){
		yield return null;
	}

	IEnumerator EndDayCycle(){
		//GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ().SetScreenOverlayColor (Color.black);
		GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ().StartFade (Color.black, 2.0f);
		yield return new WaitForSeconds (2.0f);
		Sun.transform.Rotate( new Vector3(7.633f,-201.307f,-153.5f));
		//advance time
		//fade in
		GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ().StartFade (Color.clear, 2.0f);
	}


}
