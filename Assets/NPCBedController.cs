using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBedController : MonoBehaviour {
	//controller to, fuck, handle the beds. because why the fuck not???
	// Use this for initialization
	public GameObject[] npcBeds;

	void Start () {
		//AssignBeds();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AssignBeds(){
		GameObject[] beds = GameObject.FindGameObjectsWithTag ("bed");
		GameObject[] npcs = GameObject.FindGameObjectsWithTag("WorkerNPC");
		for(int i = 0; i < beds.Length; i++){
			//if (npcs [i] != null) {
				npcs [i].GetComponent<NPCstats> ().bedIndex = i;
			//}
		}
	}
}
