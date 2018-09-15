using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour {

	public GameObject Holder;
	GameObject player;

	public bool LoadLevelOnEnter;

	public bool newGameStarting = true;

	// Use this for initialization
	void Start () {
		if (!LoadLevelOnEnter && !newGameStarting) {
			PositionPlayerandNPCsForCaveExit();
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PositionPlayerandNPCsForCaveExit(){
		player = GameObject.Find ("Player");
		//Holder =  //GameObject.Find ("NPCHolder");
		player.transform.position = transform.position;

		GameObject[] Miners = GameObject.FindGameObjectsWithTag ("WorkerNPC");

		for (int i = 0; i < Miners.Length; i++) {
			//print ("sending a miner to position of " + transform.position);
			Vector3 exitPosition = new Vector3 (2.28f, 0.29f, -4.52f);
			Miners [i].transform.position = exitPosition;
			print ("thier position " + Miners [i].transform.position);
		}
		//moveNPCTeamToPoint (Holder, transform.position);
	}

	void moveNPCTeamToPoint(GameObject Holder, Vector3 Location){
		GameObject[] Miners = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		Holder.GetComponent<PlayerAndNPCSpawner> ().setPoint (Location);

		for (int i = 0; i < Miners.Length; i++) {
			//Miners[i].GetComponent<AIStateMachine>().ResetNPCVariables();
			Holder.GetComponent<PlayerAndNPCSpawner> ().addNPC(Miners [i]);
		}

	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player" || other.tag == "WorkerNPC") {
			//TEMP 
			//Holder.GetComponent<PlayerAndNPCSpawner> ().placeNextNPC ();
		} 
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" && LoadLevelOnEnter) {
			SceneManager.LoadScene ("testMapScene");
			LoadLevelOnEnter = false;
		}
	}
}
