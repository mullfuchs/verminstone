﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CampEventController : MonoBehaviour {

	public bool ForceDayToEndByPressingY = true;

	public GameObject Barracks;
	public GameObject caveStagingArea;

    public GameObject MessHall;
    public GameObject currentStagingArea;

	//public GameObject[] gatheringAreaLocationObjects;

	public GameObject[] NPCMiners = new GameObject[5];
	public GameObject[] NPCCarriers = new GameObject[5];

	private GameObject[] AllNPCs;
	private GameObject canvas;

	public GameObject caveExit;
	public GameObject caveEntrance;

    private ExitCaveNPCEventController exitCaveInstance;
	private CaveEntrance caveExitObject;

	public static CampEventController instance = null;

	public GameObject Sun;
	private float DayCycleClockTime;
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find ("Canvas");

		AllNPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
		exitCaveInstance = caveExit.GetComponent<ExitCaveNPCEventController> (); 
		caveExitObject = caveExit.GetComponent<CaveEntrance> ();

		//gameObject.GetComponent<CampPopulationController> ().SpawnNewPlayerAndNPCSquad ();
	}

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		refreshReferences ();
		//repopulate 
	}

	public void refreshReferences(){
		//print ("refreshing references");
		canvas = GameObject.Find ("Canvas");
		AllNPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
		MessHall = GameObject.Find ("Mess Hall");
		exitCaveInstance = caveExit.GetComponent<ExitCaveNPCEventController> (); 
		caveExitObject = caveExit.GetComponent<CaveEntrance> ();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Y)){
			SendNPCsToStagingArea ();
		}
	}

	public void SendNPCsToBarracks(){
        CleanUpGatheringArea(currentStagingArea);
		AllNPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
		SendNPCsToArea (AllNPCs, Barracks);
        currentStagingArea = Barracks;
	}

	public void SendNPCsToStagingArea(){
        CleanUpGatheringArea(currentStagingArea);
		SendNPCsToArea (AllNPCs, caveStagingArea);
        currentStagingArea = caveStagingArea;
	}

    public void SendAllNPCsToArea(GameObject area)
    {
        CleanUpGatheringArea(currentStagingArea);
        SendNPCsToArea(AllNPCs, area);
        currentStagingArea = area;
    }

	public void EndDay(){
		StartCoroutine (EndDayCycle ());
	}

	void SendNPCsToArea(GameObject[] NPCGroup, GameObject target){
		GameObject[] spots = InitializeGatheringArea (target);
		for (int i = 0; i < NPCGroup.Length; i++) {
			NPCGroup [i].GetComponent<AIStateMachine> ().ResetNPCVariables ();
			NPCGroup [i].GetComponent<AIStateMachine> ().AddTargetForNPC (spots[i]);
		}
	}

	public void SendNPCGroupToTarget(GameObject[] NPCGroup, GameObject target)
    {
        for (int i = 0; i < NPCGroup.Length; i++)
        {
            NPCGroup[i].GetComponent<AIStateMachine>().AddTargetForNPC(target);
        }
    }

	public void ClearAllNPCTargts ()
	{
		for (int i = 0; i < AllNPCs.Length; i++)
		{
			AllNPCs[i].GetComponent<AIStateMachine>().ResetNPCVariables();
		}
	}


    void CleanUpGatheringArea(GameObject GatheringAreaObject)
    {
        if(GatheringAreaObject != null)
        {
            foreach (Transform child in GatheringAreaObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

    }

    GameObject[] InitializeGatheringArea(GameObject gatheringAreaObject){
		
		GameObject[] gatheringAreaLocationObjects;
		int size = AllNPCs.Length;
		//print ("creating gathering area, size: " + size);
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
		GameObject.Find("Player").GetComponent<NPCTeamHandler>().resetNPCTargets();
		GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ().StartFade (Color.black, 2.0f);
		GameObject.Find ("CampEventController").GetComponent<VStoneEconomyObject> ().resetDailyTotal ();
		yield return new WaitForSeconds (2.0f);
        //Sun.transform.Rotate( new Vector3(7.633f,-201.307f,-153.5f));
        //advance time
        gameObject.GetComponent<CampNarrativeController>().day += 1;
		gameObject.GetComponent<CampNarrativeController>().timeOfDay = CampNarrativeController.timePeriod.Morning;
		gameObject.GetComponent<CampNarrativeController> ().AdvanceDialogDayOfNPCs ();
		//gameObject.GetComponent<CampNarrativeController> ().AdvanceDialoyDayOfKeyNPCs ();
		gameObject.GetComponent<CampNarrativeController> ().UpdateNPCNarratives ();
		//gameObject.GetComponent<CampNarrativeController> ().UpdateKeyNPCNarratives ();
		gameObject.GetComponent<VStoneEconomyObject>().IncreaseDailyQuota(gameObject.GetComponent<CampNarrativeController>().day);

        //fade in
        caveEntrance.GetComponent<CaveEntrance>().LoadLevelOnEnter = true;
        GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ().StartFade (Color.clear, 2.0f);
        SendAllNPCsToArea(MessHall);
        
	}

	public void EnterCaveSequence(){
		//set cave entrance object to be inactive
		//caveExitObject.LoadLevelOnEnter = false;

		caveEntrance.SetActive(false);
		caveExit.SetActive(false);
	}

	public void ExitCaveSequence(){
		print ("exiting cave");
		gameObject.GetComponent<CampNarrativeController>().timeOfDay = CampNarrativeController.timePeriod.Evening;
		gameObject.GetComponent<CampNarrativeController> ().UpdateNPCNarratives ();
		//gameObject.GetComponent<CampNarrativeController> ().UpdateKeyNPCNarratives ();
		//set the object to be active
		caveExit.SetActive(true);
		caveEntrance.SetActive (true);
        //then do this
        //SendNPCsToArea(AllNPCs, caveStagingArea);
        caveExitObject.PositionPlayerandNPCsForCaveExit ();
        

		exitCaveInstance.doCaveExitEvent (); //try doing this when player steps off exit
    }

	public void StartMessHallSequence(){
		canvas.GetComponent<NPCFoodDistroUIController> ().CreateAndDisplayNPCcards ();
	}

	public void EndMessHallSequence(){
		canvas.GetComponent<NPCFoodDistroUIController> ().cleanUpFoodUI ();
	}

    public void StartEquipAreaSequence()
    {
        canvas.GetComponent<EquipUIController>().CreateAndDisplayNPCcards();
        canvas.GetComponent<EquipUIController>().CreateAndDisplayItemCards();
        //create and display equip UI
    }

	public void EndEquipAreaSequence(){
		canvas.GetComponent<EquipUIController> ().cleanUpItemAndNPCCards ();
	}



}
