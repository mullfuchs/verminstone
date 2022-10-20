using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour {

	public GameObject Holder;
	GameObject player;

	public bool LoadLevelOnEnter;

	public bool newGameStarting = true;

    public GameObject[] NPCExitPositions;

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

        //hmm, don't wanna use this anymore
		Holder.GetComponent<PlayerAndNPCSpawner> ().placeAllNPCs(NPCExitPositions);

        //i guess iterate through the thing here???

        player.GetComponent<PowerObject>().setGlowLight(false);

        /*
		GameObject[] Miners = GameObject.FindGameObjectsWithTag ("WorkerNPC");

		print ("moving npcs to position: " + player.transform.position);
		for (int i = 0; i < Miners.Length; i++) {
			Miners [i].transform.position = player.transform.position; //player.transform.position;
			print ("thier position " + Miners [i].transform.position);
		}
		*/

        //
    }

	void moveNPCTeamToPoint(GameObject Holder, Vector3 Location){
		GameObject[] Miners = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		Holder.GetComponent<PlayerAndNPCSpawner> ().setPoint (Location);

		for (int i = 0; i < Miners.Length; i++) {
			//Miners[i].GetComponent<AIStateMachine>().ResetNPCVariables();
			Holder.GetComponent<PlayerAndNPCSpawner> ().addNPC(Miners [i]);
		}

	}

	void massMoveNPCToPlayer(){
		GameObject[] Miners = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		player = GameObject.Find ("Player");

		//print ("moving npcs to position: " + player.transform.position);
		for (int i = 0; i < Miners.Length; i++) {
			Miners [i].transform.position = player.transform.position; //player.transform.position;
			//print ("thier position " + Miners [i].transform.position);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {
			//TEMP 
			//Holder.GetComponent<PlayerAndNPCSpawner> ().placeAllNPCs ();

		} 
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player" && LoadLevelOnEnter && Input.GetButtonDown("Action")) {
            other.gameObject.GetComponent<PowerObject>().setGlowLight(true);
			SceneManager.LoadScene ("testMapScene");
			LoadLevelOnEnter = false;
		}
	}

    public void enterCaveAndStartRun()
    {
        //other.gameObject.GetComponent<PowerObject>().setGlowLight(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PowerObject>().setGlowLight(true);
        player.GetComponentInChildren<ShootOnAxisInput>().canShoot = true;
        player.GetComponent<PowerObject>().canHeal = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<NPCTeamHandler>().rebuildNPCLists();
        SceneManager.LoadScene("testMapScene");
        LoadLevelOnEnter = false;
    }
}
