using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageTrigger : MonoBehaviour {

	GameObject NPCHolder;

	// Use this for initialization
	void Start () {
		NPCHolder = GameObject.Find ("NPCHolder");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player" || other.tag == "Carrier" || other.tag == "Miner") {
			//Debug.Log ("Stepped off exit trigger");
			//if(gameObject.GetComponent<ChangeFloorOnEnter>());
			gameObject.GetComponent<ChangeFloorOnEnter> ().CanSwapLevels = true;
			NPCHolder.GetComponent<PlayerAndNPCSpawner> ().placeNextNPC ();
		} 
	}


}
