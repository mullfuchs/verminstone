using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//spawn in player and NPCs when they die or whatever

public class CampPopulationController : MonoBehaviour {

	public bool IsNewGame = false;

	public int NPCSquadSize = 5;

	public GameObject PlayerPrefab;
	public GameObject NPCPrefab;

	public Transform PlayerSpawnPoint;
	public Transform NPCSpawnPoint;
    public Transform NonWorkerNPCSpawnPoint;

	public TextAsset NPCStatCSV;

	public GameObject blankNPCPrefab;
	public GameObject[] NPCBodyTypes;
	public Material[] NPCBodyMaterials;

    public NPCStatRecord[] NPCRecords;

	public int NPCRecordStatIndex = 0;

    // Use this for initialization
    void Start () {
		NPCSpawnPoint = GameObject.Find ("NPCSpawn").transform;
		PlayerSpawnPoint = GameObject.Find ("PlayerSpawn").transform;

		GameObject startGameObj = GameObject.Find("StartGameController");

		NPCRecords = loadNPCStatsFromCSV().ToArray();

        if (startGameObj != null && startGameObj.GetComponent<StartGameController>().loadGameFromSave == true)
		{
			print("loading game from save");
			IsNewGame = false;
            //this is also being looked at in the game save controller so watch it
            startGameObj.GetComponent<StartGameController>().loadGameFromSave = false;

        }

		if (IsNewGame == true)
		{
			//create new NPC Pool (load from CSV, shuffle)
			//NPCRecords = loadNPCStatsFromCSV().ToArray();

			SpawnNewPlayerAndNPCSquad();
			IsNewGame = false;
		}

        

	}

	void Awake() {

			
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnNewPlayerAndNPCSquad(){
		print ("spawning new npcs and player");
		SpawnPlayerPrefab ();
		for (int i = 0; i < NPCSquadSize; i++) {
			//GameObject npc = SpawnNPCPrefab ();
			GameObject blankNPC = Instantiate (blankNPCPrefab, NPCSpawnPoint.position, Quaternion.identity);
			AssociateNewNPCWithNPCStatRecord (blankNPC);
			giveNPCCorrectBodyAndTexture (blankNPC);
		}
        while(NPCRecordStatIndex < NPCRecords.Length)
        {
            SpawnNonWorkerNPC();
        }

	}

    public void SpawnNonWorkerNPC()
    {
        GameObject blankNPC = Instantiate(blankNPCPrefab, NonWorkerNPCSpawnPoint.position, Quaternion.identity);
        AssociateNewNPCWithNPCStatRecord(blankNPC);
        giveNPCCorrectBodyAndTexture(blankNPC);
        blankNPC.tag = "dialog_npc";
    }

	public void ReplaceDeadNPCs(){
		//get count of npcs
		int npcCount = GameObject.FindGameObjectsWithTag("WorkerNPC").Length;
		//how many do we need?
		int replacementNPCCount = NPCSquadSize - npcCount;
		//spawn em in baby

		CampNarrativeController narrativeController = gameObject.GetComponent<CampNarrativeController> ();

		for (int i = 0; i < replacementNPCCount; i++) {
            GameObject[] nonWorkers = GameObject.FindGameObjectsWithTag("dialog_npc");
            int randIndex = UnityEngine.Random.Range(0, nonWorkers.Length);
            nonWorkers[randIndex].tag = "WorkerNPC";
		}
		//then set up a narrative for it
	}

    public void shuffleNPCAssignments()
    {

        //retrying with lists
       // List<GameObject> workerNPCs = new List<GameObject>();
      //  List<GameObject> dialogNPCs = new List<GameObject>();

      //  List<GameObject> combinedNPCs = workerNPCs.AddRange(dialogNPCs);


        GameObject[] workerNPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        GameObject[] dialogNPCs = GameObject.FindGameObjectsWithTag("dialog_npc");
        //combine and set all tags to None
        GameObject[] combined = new GameObject[workerNPCs.Length + dialogNPCs.Length];
        Array.Copy(workerNPCs, combined, workerNPCs.Length);
        Array.Copy(dialogNPCs, 0, combined, workerNPCs.Length, dialogNPCs.Length);
        //randomize order?
        ShuffleArray(combined);
        //set first five to worker npc, set next 5 to dialognpc

        int counter = 0;
        foreach(GameObject npc in combined)
        {
            if(counter >= 5)
            {
                npc.tag = "dialog_npc";
            }
            else
            {
                npc.tag = "WorkerNPC";
                counter++;
            }
        }
        /*
        for(int i = 0; i <= 4; i++)
        {
            combined[i].tag = "WorkerNPC";
        }

        for(int i = 5; i <= combined.Length; i++)
        {
            combined[i].tag = "dialog_npc";
        }
        */

    }

	public void LoadNPCFromSave(string name, float healthpoints, Vector3 position, int daysTalkedTo, int scriptIndex, NPCStatRecord statRecord, bool isWorker, bool isEscaping){
		GameObject npc = Instantiate (blankNPCPrefab, position, Quaternion.identity);

		NPCstats stats = npc.GetComponent<NPCstats> ();
		stats.name = name;
		stats.daysTalkedTo = daysTalkedTo;
		stats.NPCScriptIndex = scriptIndex;
		stats.health = healthpoints;
		CampNarrativeController narrativeController = gameObject.GetComponent<CampNarrativeController> ();

        npc.GetComponent<health>().healthPoints = healthpoints;

		narrativeController.SetUpNewNPCNarrative (npc, scriptIndex);
		print ("Attempting to load " + stats.NPCName);
		stats.loadNPCStatFromRecord (statRecord);
		giveNPCCorrectBodyAndTexture (npc);

        if (!isWorker)
        {
            npc.tag = "dialog_npc";
        }
        npc.GetComponent<Yarn.Unity.Example.NPC>().canTalkTo = false;

        npc.GetComponent<NPCOverworldController>().isEscaping = isEscaping;

        //npc.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().agent.Warp ( position );
    }

    public void LoadPlayerFromSave(Vector3 position, float healthpoints)
    {
        GameObject player = SpawnPlayerPrefab();
        player.transform.position = position;
        player.GetComponent<health>().healthPoints = healthpoints;
    }

	private GameObject SpawnPlayerPrefab(){
		GameObject Player = Instantiate (PlayerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
		Player.name = "Player";
        return Player;
	}

	private GameObject SpawnNPCPrefab(){
		GameObject NPC = Instantiate (NPCPrefab, NPCSpawnPoint.position, Quaternion.identity);
		return NPC;
	}
		

	private List<NPCStatRecord> loadNPCStatsFromCSV(){

		string[] NPCStats = NPCStatCSV.text.Split('\n');
		List<NPCStatRecord> NPCStatRecords = new List<NPCStatRecord> ();

		for (int i = 1; i < NPCStats.Length; i++) {
			//skipping first entry in CSV, which contains labels
			string[] stats = NPCStats[i].Split (',');

			NPCStatRecord npcRecord = new NPCStatRecord();
			npcRecord.Name = stats [0];
            npcRecord.AnimalType = stats[1];
            npcRecord.FurType = ConvertStringToInt(stats[2]);
            npcRecord.BaseHP = ConvertStringToInt( stats [3]);
			npcRecord.Attack = ConvertStringToInt( stats [4]);
			npcRecord.Defense = ConvertStringToInt( stats [5]);
			npcRecord.Bravery = ConvertStringToInt( stats [6]);
			npcRecord.RunSpeed = ConvertStringToInt( stats [7]);
			NPCStatRecords.Add (npcRecord);
            print("added record for " + npcRecord.Name);
		}

        return NPCStatRecords;
	}

	private int ConvertStringToInt(string intString)
	{
		int i = 0;
		try
		{
			i = System.Convert.ToInt32(intString);
		}
		catch (FormatException)
		{
			print ("NPCstat CSV import failed, format exception");
		}
		catch (OverflowException)
		{
			// the OverflowException is thrown when the string is a valid integer, 
			// but is too large for a 32 bit integer.  Use Convert.ToInt64 in
			// this case.
			print("NPCstat CSV import failed, overflow exception");
		}
		return i;
	}

    void ShuffleArray(GameObject[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            GameObject tmp = texts[t];
            int r = UnityEngine.Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }

    public void WarpNPCsToBed()
    {
        GameObject[] workerNPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        GameObject[] dialogNPCs = GameObject.FindGameObjectsWithTag("dialog_npc");

        foreach(GameObject npc in workerNPCs)
        {
            //get npc bed location
            Vector3 bedPosition = gameObject.GetComponent<NPCBedController>().npcBeds[npc.GetComponent<NPCstats>().bedIndex].transform.position;
            npc.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(bedPosition);
        }

        foreach (GameObject npc in dialogNPCs)
        {
            //get npc bed location
            Vector3 bedPosition = gameObject.GetComponent<NPCBedController>().npcBeds[npc.GetComponent<NPCstats>().bedIndex].transform.position;
            npc.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(bedPosition);
        }

    }

    private void AssociateNewNPCWithNPCStatRecord(GameObject npc){
		npc.GetComponent<NPCstats> ().loadNPCStatFromRecord (NPCRecords [NPCRecordStatIndex]);
		NPCRecordStatIndex++;
	}

	private void giveNPCCorrectBodyAndTexture(GameObject npc){
        npc.name = npc.GetComponent<NPCstats>().statObject.Name;
        string NPCType = npc.GetComponent<NPCstats> ().statObject.AnimalType;
		int furType = npc.GetComponent<NPCstats> ().statObject.FurType;
		GameObject body;
        
        switch (NPCType)
        {
            case "rabbit":
                body = Instantiate(NPCBodyTypes[0], npc.transform);
                break;
            case "fox":
                body = Instantiate(NPCBodyTypes[1], npc.transform);
                break;
            case "cat":
                body = Instantiate(NPCBodyTypes[2], npc.transform);
                break;
            case "dog":
                body = Instantiate(NPCBodyTypes[3], npc.transform);
                break;
            default:
                body = Instantiate(NPCBodyTypes[0], npc.transform);
                break;
        }


		body.GetComponentInChildren<SkinnedMeshRenderer> ().material = NPCBodyMaterials [furType];

		npc.GetComponent<Animator> ().avatar = body.GetComponent<Animator> ().avatar;

	}
}

[System.Serializable]
public class NPCStatRecord{
	public string Name;
	public string AnimalType;
	public int FurType;
	public int BaseHP;
	public int Attack;
	public int Defense;
	public int Bravery;
	public int RunSpeed;
}