using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class CampEventController : MonoBehaviour {

	public bool ForceDayToEndByPressingY = true;

	public GameObject Barracks;
	public GameObject caveStagingArea;

    public GameObject MessHall;
    public GameObject currentStagingArea;

	//public GameObject[] gatheringAreaLocationObjects;

	public GameObject[] NPCMiners = new GameObject[5];
	public GameObject[] NPCCarriers = new GameObject[5];

    public GameObject[] NonWorkerNPCs;

	private GameObject[] AllNPCs;
	private GameObject canvas;

	public GameObject caveExit;
	public GameObject caveEntrance;

    private ExitCaveNPCEventController exitCaveInstance;
	private CaveEntrance caveExitObject;

	public static CampEventController instance = null;

	public GameObject Sun;
	private float DayCycleClockTime;

	public int DaysToFinishGame = 10;

	public int day = 1;

	private GameObject[] NPCbeds;
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

	public void SendAllNPCsToAGameObject(GameObject target)
	{
		
	}

    public void HideNonWorkerNPCs()
    {
        NonWorkerNPCs = GameObject.FindGameObjectsWithTag("dialog_npc");
        foreach(GameObject g in NonWorkerNPCs)
        {
            g.SetActive(false);
        }
    }

    public void UnHideWorkerNPCs()
    {
        if(NonWorkerNPCs.Length > 0)
        {
            foreach (GameObject g in NonWorkerNPCs)
            {
                if(g != null)
                {
                    g.SetActive(true);
                    g.GetComponent<NavMeshAgent>().Warp(gameObject.GetComponent<CampPopulationController>().NonWorkerNPCSpawnPoint.transform.position);
                }
            }
        }
    }

    public void NPCDialogEnabled(bool enabled)
    {
        AllNPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        foreach(GameObject npc in AllNPCs)
        {
            npc.GetComponentInChildren<TriggerPromptOnEnter>().canTalkToNPC = enabled;
        }
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
            NPCGroup[i].GetComponent<AIStateMachine>().SendNPCToObject(target);
        }
    }

    public void SendNPCGroupToTargetHARDSEND(GameObject[] NPCGroup, GameObject target)
    {
        for (int i = 0; i < NPCGroup.Length; i++)
        {
            NPCGroup[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination( target.transform.position );
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
		gameObject.GetComponent<GameSaveController>().SaveGame();
        
		GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ().StartFade (Color.black, 2.0f);
		GameObject.Find ("CampEventController").GetComponent<VStoneEconomyObject> ().resetDailyTotal ();

		yield return new WaitForSeconds (2.0f);
        //Sun.transform.Rotate( new Vector3(7.633f,-201.307f,-153.5f));
        //advance time
		day += 1;
		GameObject.Find ("DayCounter").GetComponent<UnityEngine.UI.Text> ().text = "Day:" + day;
        gameObject.GetComponent<CampNarrativeController>().day += 1;
		gameObject.GetComponent<CampNarrativeController>().timeOfDay = CampNarrativeController.timePeriod.Morning;

		if (day >= DaysToFinishGame) {
			print ("you won the game!");
			SceneManager.LoadScene ("tempEndGame");
		}
        //hard coding escape quest to start at a certain day
        if(day >= 1 && GameObject.Find("CampAreaSecretEscape") == null )
        {
           // print("trying to turn on camp area secret escape");
            GameObject.Find("GameQuestObjects").GetComponent<CampQuestController>().StartQuest("CampAreaSecretEscape");
        }

        if(!gameObject.GetComponent<CampNarrativeController>().RunDreamForDay(day))
        {
            StartDay();
        }

	}

	public void StartDay(){

        refreshReferences();
		gameObject.GetComponent<CampPopulationController> ().ReplaceDeadNPCs ();

		gameObject.GetComponent<CampNarrativeController> ().AdvanceDialogDayOfNPCs ();
		//gameObject.GetComponent<CampNarrativeController> ().AdvanceDialoyDayOfKeyNPCs ();
		gameObject.GetComponent<CampNarrativeController> ().UpdateNPCNarratives ();
		//gameObject.GetComponent<CampNarrativeController> ().UpdateKeyNPCNarratives ();
		gameObject.GetComponent<VStoneEconomyObject>().IncreaseDailyQuota(gameObject.GetComponent<CampEventController>().day);

		//fade in
		caveEntrance.GetComponent<CaveEntrance>().LoadLevelOnEnter = true;

        print("sending npcs to food table");
        SendNPCGroupToTarget(GameObject.FindGameObjectsWithTag("WorkerNPC"), GameObject.Find("FoodTable"));
        GameObject.Find("Player").GetComponent<PlayerEventController>().canEndDay = true;

        GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ().StartFade (Color.clear, 2.0f);
        Yarn.Unity.Example.NPC[] NPCTalkObjects = FindObjectsOfType<Yarn.Unity.Example.NPC>();
        foreach(Yarn.Unity.Example.NPC npc in NPCTalkObjects)
        {
            npc.canTalkTo = true;
        }
        gameObject.GetComponent<CampInventoryController>().EnableShopKeeper(true);

	}

	public void EnterCaveSequence(){
        //set cave entrance object to be inactive
        //caveExitObject.LoadLevelOnEnter = false;
        HideNonWorkerNPCs();
        GameObject.Find("Player").GetComponent<NPCTeamHandler>().rebuildNPCLists();
        GameObject.Find("Player").GetComponent<PlayerEventController>().dialogOpened = false;
        gameObject.GetComponent<VStoneEconomyObject>().SetDailyQuota();
        caveEntrance.GetComponent<CaveEntrance>().enterCaveAndStartRun();
		//caveEntrance.SetActive(false);
		caveExit.SetActive(false);
	}

	public void ExitCaveSequence(){
		print ("exiting cave");
        UnHideWorkerNPCs();
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
        if(canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }
		canvas.GetComponent<NPCFoodDistroUIController> ().CreateAndDisplayNPCcards ();
	}

	public void EndMessHallSequence(){
		if (gameObject.GetComponent<CampNarrativeController> ().timeOfDay == CampNarrativeController.timePeriod.Morning) {
			print ("Sending npcs to prestage area");
            ClearAllNPCTargts();

            SendNPCGroupToTargetHARDSEND(GameObject.FindGameObjectsWithTag("WorkerNPC"), GameObject.Find("bigdoor"));
		}
		if(canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }	
		canvas.GetComponent<NPCFoodDistroUIController> ().cleanUpFoodUI ();
        GameObject.Find("Player").GetComponent<PlayerEventController>().dialogOpened = false;
    }

    public void StartEquipAreaSequence()
    {
        canvas = GameObject.Find("Canvas");
        //check here if there's been enough vstone collected
        VStoneEconomyObject vEco = gameObject.GetComponent<VStoneEconomyObject>();
		if (!vEco.meetsDailyQuota (vEco.getDailyTotal ())) {
			canvas.GetComponent<EquipUIController> ().CreateAndDisplayItemCards ();
			canvas.GetComponent<EquipUIController> ().CreateAndDisplayNPCcards ();
		}
        //create and display equip UI
        //disable playerinput
    }

	public void EndEquipAreaSequence(){
        GameObject.Find("Player").GetComponent<PlayerEventController>().dialogOpened = false;
        canvas.GetComponent<EquipUIController> ().cleanUpItemAndNPCCards ();
	}

    public void StartTunnelDigSequence()
    {

        canvas.GetComponent<EscapeUIController>().CreateAndDisplayNPCcards();
    }

    public void EndTunnelDigSequence()
    {
        canvas.GetComponent<EscapeUIController>().CleanUpEscapeUI();
        GameObject.Find("Player").GetComponent<PlayerEventController>().dialogOpened = false;
    }

    public void MakeNonWorkernPCsIdle()
    {
        if(NonWorkerNPCs.Length > 0)
        {
            foreach(GameObject npc in NonWorkerNPCs)
            {
                npc.GetComponent<NPCOverworldController>().idling = true;
            }
        }
    }

}
