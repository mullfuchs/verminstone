using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//spawn in player and NPCs when they die or whatever

public class CampPopulationController : MonoBehaviour {



	public bool IsNewGame = false;

	public int NPCSquadSize = 5;

	public GameObject PlayerPrefab;
	public GameObject NPCPrefab;

	public Transform PlayerSpawnPoint;
	public Transform NPCSpawnPoint;

	// Use this for initialization
	void Start () {

		/*
		if (IsNewGame) {
			SpawnNewPlayerAndNPCSquad ();
			IsNewGame = false;
		} else {

		}


		if (GameObject.Find ("StartGameController") != null &&
		   GameObject.Find ("StartGameController").GetComponent<StartGameController> ().loadGameFromSave) {
			IsNewGame = false;
			//this is also being looked at in the game save controller so watch it
		} else {
			SpawnNewPlayerAndNPCSquad ();
			IsNewGame = false;
		}

		*/
	}

	void Awake() {
		NPCSpawnPoint = GameObject.Find ("NPCSpawn").transform;
		PlayerSpawnPoint = GameObject.Find ("PlayerSpawn").transform;

		if (IsNewGame == true)
		{
			SpawnNewPlayerAndNPCSquad();
			IsNewGame = false;
		}

        GameObject startGameObj = GameObject.Find("StartGameController");
        if (startGameObj != null && startGameObj.GetComponent<StartGameController>().loadGameFromSave == true)
        {
            print("loading game from save");
            IsNewGame = false;
            //this is also being looked at in the game save controller so watch it
        }
			
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnNewPlayerAndNPCSquad(){
		print ("spawning new npcs and player");
		SpawnPlayerPrefab ();
		for (int i = 0; i < NPCSquadSize; i++) {
			SpawnNPCPrefab ();
		}
	}

	public void ReplaceDeadNPCs(){
		//get count of npcs
		int npcCount = GameObject.FindGameObjectsWithTag("WorkerNPC").Length;
		//how many do we need?
		int replacementNPCCount = NPCSquadSize - npcCount;
		//spawn em in baby

		CampNarrativeController narrativeController = gameObject.GetComponent<CampNarrativeController> ();

		for (int i = 0; i < replacementNPCCount; i++) {
			GameObject npc = SpawnNPCPrefab ();
			narrativeController.SetUpNewNPCNarrative (npc);
		}
		//then set up a narrative for it
	}

	public void LoadNPCFromSave(string name, float healthpoints, Vector3 position, int daysTalkedTo, int scriptIndex){
		GameObject npc = SpawnNPCPrefab ();
		npc.transform.position = position;
		NPCstats stats = npc.GetComponent<NPCstats> ();
		stats.name = name;
		stats.daysTalkedTo = daysTalkedTo;
		stats.NPCScriptIndex = scriptIndex;
		stats.health = healthpoints;
		CampNarrativeController narrativeController = gameObject.GetComponent<CampNarrativeController> ();

		narrativeController.SetUpNewNPCNarrative (npc, scriptIndex);
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
}
