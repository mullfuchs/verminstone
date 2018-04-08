using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//spawn in player and NPCs when they die or whatever

public class CampPopulationController : MonoBehaviour {



	private bool IsNewGame = true;

	public int NPCSquadSize = 5;

	public GameObject PlayerPrefab;
	public GameObject NPCPrefab;

	public Transform PlayerSpawnPoint;
	public Transform NPCSpawnPoint;

	// Use this for initialization
	void Start () {
		if (IsNewGame) {
			SpawnNewPlayerAndNPCSquad ();
			IsNewGame = false;
		}
		IsNewGame = false;
	}

	void Awake() {

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
	
	}

	private void SpawnPlayerPrefab(){
		GameObject Player = Instantiate (PlayerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
		Player.name = "Player";
	}

	private void SpawnNPCPrefab(){
		GameObject NPC = Instantiate (NPCPrefab, NPCSpawnPoint.position, Quaternion.identity);

	}
}
