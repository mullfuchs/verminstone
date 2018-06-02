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
		moveNPCTeamToPoint (Holder, transform.position);
	}

	void moveNPCTeamToPoint(GameObject Holder, Vector3 Location){
		GameObject[] Miners = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		//GameObject[] Miners = player.GetComponent<NPCTeamHandler> ().NPCMiners;
		Holder.GetComponent<PlayerAndNPCSpawner> ().setPoint (Location);


		for (int i = 0; i < Miners.Length; i++) {
			//Miners [i].GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
			//Miners [i].transform.position = point.transform.position;
			//Debug.Log ("Enqued");

			Holder.GetComponent<PlayerAndNPCSpawner> ().addNPC(Miners [i]);
			//Miners [i].SetActive (false);

			//Miners [i].GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = true;
		}

	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player" || other.tag == "WorkerNPC") {
			Holder.GetComponent<PlayerAndNPCSpawner> ().placeNextNPC ();
		} 
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" && LoadLevelOnEnter) {
			SceneManager.LoadScene ("testMapScene");
			LoadLevelOnEnter = false;
		}
	}
}
