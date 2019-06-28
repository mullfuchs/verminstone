using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//spawn in player and NPCs when they die or whatever

public class CampPopulationController : MonoBehaviour {



	public bool IsNewGame = true;

	public int NPCSquadSize = 5;

	public GameObject PlayerPrefab;
	public GameObject NPCPrefab;

	public Transform PlayerSpawnPoint;
	public Transform NPCSpawnPoint;

	// Use this for initialization
	void Start () {
		if (GameObject.Find ("StartGameController").GetComponent<StartGameController> ().loadGameFromSave) {
			IsNewGame = false;
			//this is also being looked at in the game save controller so watch it
		}


		if (IsNewGame) {
			SpawnNewPlayerAndNPCSquad ();
			IsNewGame = false;
		} else {
			
		}

	}

	void Awake() {
		NPCSpawnPoint = GameObject.Find ("NPCSpawn").transform;
		PlayerSpawnPoint = GameObject.Find ("PlayerSpawn").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnNewPlayerAndNPCSquad(){
		print ("spawning npcs and player");
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

	private void SpawnPlayerPrefab(){
		GameObject Player = Instantiate (PlayerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
		Player.name = "Player";
	}

	private GameObject SpawnNPCPrefab(){
		GameObject NPC = Instantiate (NPCPrefab, NPCSpawnPoint.position, Quaternion.identity);
		return NPC;
	}
}
