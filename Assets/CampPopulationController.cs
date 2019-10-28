﻿using System.Collections;
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

	public TextAsset NPCStatCSV;

	// Use this for initialization
	void Start () {
		NPCSpawnPoint = GameObject.Find ("NPCSpawn").transform;
		PlayerSpawnPoint = GameObject.Find ("PlayerSpawn").transform;

		GameObject startGameObj = GameObject.Find("StartGameController");
		if (startGameObj != null && startGameObj.GetComponent<StartGameController>().loadGameFromSave == true)
		{
			print("loading game from save");
			IsNewGame = false;
			//this is also being looked at in the game save controller so watch it
		}

		if (IsNewGame == true)
		{
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
       
		npc.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().agent.Warp ( position );

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

	//npc stat work should go here, I think
	//how do I want to set up NPC stats? well, I could use a CSV that holds all the values, and
	//use that to easily set up individual NPCs.

	private void loadNPCStatsFromCSV(){
		//load text asset
		//parse CSV ???

		string[] NPCStats = NPCStatCSV.text.Split('\n');
		for (int i = 1; i < NPCStats.Length; i++) {
			//skipping first entry in CSV, which contains labels
			string[] stats = NPCStats[i].Split (',');
		}
			
	}
}

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