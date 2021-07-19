using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBedController : MonoBehaviour {
	//controller to, fuck, handle the beds. because why the fuck not???
	// Use this for initialization
	public GameObject[] npcBeds;

	void Start () {

		AssignBeds();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AssignBeds(){
		npcBeds = GameObject.FindGameObjectsWithTag("bed");
		GameObject[] npcs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        GameObject[] nonWorkerNPCs = GameObject.FindGameObjectsWithTag("dialog_npc");
        GameObject[] combinedNPCs = new GameObject[npcs.Length + nonWorkerNPCs.Length];
        System.Array.Copy(npcs, combinedNPCs, npcs.Length);
        System.Array.Copy(nonWorkerNPCs, 0, combinedNPCs, npcs.Length, nonWorkerNPCs.Length);
        for (int i = 0; i < combinedNPCs.Length; i++){
            //if (npcs [i] != null) {
                combinedNPCs[i].GetComponent<NPCstats> ().bedIndex = i;
			//}
		}
	}
}
