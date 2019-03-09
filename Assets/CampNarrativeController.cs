using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampNarrativeController : MonoBehaviour {

    public int day = 0;

	public enum timePeriod {Morning, Evening};

	public timePeriod timeOfDay;

	public TextAsset[] NPCDialogs;

	// Use this for initialization
	void Start () {
	    //get the list of npcs
		day = 1;
		timeOfDay = timePeriod.Morning;
		SetUpNarratives();
	}

	public void SetUpNarratives(){
		GameObject[] npcs;
		npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		for(int i = 0; i < npcs.Length; i++){
			npcs [i].GetComponent<Yarn.Unity.Example.NPC> ().scriptToLoad = NPCDialogs [i];
			//set character name by, uh, getting the file name and parsing it?
			string[] dialogNamespace;
			char[] charSeparators = new char[] {'.'};
			dialogNamespace = NPCDialogs [i].name.Split (charSeparators, System.StringSplitOptions.None);
			string characterName = dialogNamespace [0];
			npcs [i].GetComponent<Yarn.Unity.Example.NPC> ().characterName = characterName;
			print ("setting dialog for char " + characterName);
		}
		UpdateNPCNarratives ();
	}

    public void UpdateNPCNarratives()
    {
		GameObject[] npcs;
		npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		foreach (GameObject npc in npcs) {
			string startnode = GetStartNode (npc.GetComponent<Yarn.Unity.Example.NPC> ().characterName, day, timeOfDay);
			print ("start node" + startnode);
			npc.GetComponent<Yarn.Unity.Example.NPC> ().talkToNode = startnode;
		}
    }
		

	public string GetStartNode(string characterName, int day, timePeriod time_of_day){
		string startNode = characterName + ".Day" + day + "." + time_of_day.ToString() + ".Start";
		return startNode;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
