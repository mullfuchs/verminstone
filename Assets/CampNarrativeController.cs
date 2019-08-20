using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampNarrativeController : MonoBehaviour {

    public int day = 0;

	public int scriptCount = 0;

	public enum timePeriod {Morning, Evening};

	public timePeriod timeOfDay;

	public TextAsset[] NPCDialogs;

	public GameObject[] KeyNPCs;

	public npcPortraitObject[] NPCPortraitObjects;

	public npcPortraitObject[] KeyNPCPortraitObjects;

	public GameDreamSequenceObject CurrentDream;

	public GameDreamSequenceObject[] DreamSequence;

	// Use this for initialization
	void Start () {
	    //get the list of npcs
		day = 1;
		timeOfDay = timePeriod.Morning;
		SetUpNarratives();
		SetUpNPCPortraits ();
	}

	public void SetUpNarratives(){
		GameObject[] npcs;
		npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		for(int i = 0; i < npcs.Length; i++){
			npcs [i].GetComponent<Yarn.Unity.Example.NPC> ().scriptToLoad = NPCDialogs [i];
			scriptCount++;
			//set character name by, uh, getting the file name and parsing it?
			string[] dialogNamespace;
			char[] charSeparators = new char[] {'.'};
			dialogNamespace = NPCDialogs [i].name.Split (charSeparators, System.StringSplitOptions.None);
			string characterName = dialogNamespace [0];
			npcs [i].GetComponent<Yarn.Unity.Example.NPC> ().characterName = characterName;
            npcs[i].GetComponent<NPCstats>().NPCName = characterName;
			npcs [i].GetComponent<NPCstats> ().NPCScriptIndex = i;
			print ("setting dialog for char " + characterName);
		}
		UpdateNPCNarratives ();
	}

	public void SetUpNPCPortraits(){
		print ("setting up portraits");
		GameObject[] npcs;
		npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		foreach (GameObject npc in npcs) {
			//get name of npc, if it exists
			//maybe make an object that holds the npc name and associated portraits
			string name = npc.GetComponent<NPCstats>().NPCName;
			foreach (npcPortraitObject portraitObject in NPCPortraitObjects) {
				if (name == portraitObject.NPCName) {
					npc.GetComponent<NPCstats> ().DialogPortraits = portraitObject.portraits;
				} else {
					//print ("No portraits found for npc: " + name);
				}
			}
		}
	}

    public void UpdateNPCNarratives()
    {
		//god fuck how does this work
		//I guess each npc has a fuckin...flag if they got talked to
		//and if it's true when the update hits then adance their "day" narrative

		GameObject[] npcs;
		npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		foreach (GameObject npc in npcs) {
			string startnode = GetStartNode (npc.GetComponent<Yarn.Unity.Example.NPC> ().characterName, npc.GetComponent<NPCstats>().daysTalkedTo, timeOfDay);
			print ("start node" + startnode);
			npc.GetComponent<Yarn.Unity.Example.NPC> ().talkToNode = startnode;
			//hack, if it's evening, reload the script, since the scene is being rebuilt;
			if (timeOfDay == timePeriod.Evening) {
				npc.GetComponent<Yarn.Unity.Example.NPC> ().LoadNPCScript();
			}
		}
    }

	public void UpdateKeyNPCNarratives()
	{
		//go thru the list
		//does that have an npc object?
		//okay set the start node I guess, fuck.
		foreach (GameObject keyNPC in KeyNPCs) {
			string startnode = GetStartNode (keyNPC.GetComponent<NPCstats> ().NPCName, keyNPC.GetComponent<NPCstats> ().daysTalkedTo, timeOfDay);
			keyNPC.GetComponent<Yarn.Unity.Example.NPC> ().talkToNode = startnode;
		}
	}

	public void AdvanceDialogDayOfNPCs(){
	//if they've been talked to, adance the days theyve been talked to by 1
		//god this sucks
		GameObject[] npcs;
		npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		foreach (GameObject npc in npcs) {
			if (npc.GetComponent<NPCstats> ().hasBeenTalkedToToday) {
				npc.GetComponent<NPCstats> ().daysTalkedTo += 1;
			}
		}
	}

	public void AdvanceDialoyDayOfKeyNPCs(){
		foreach (GameObject keyNPC in KeyNPCs) {
			if (keyNPC.GetComponent<NPCstats> ().hasBeenTalkedToToday) {
				keyNPC.GetComponent<NPCstats> ().daysTalkedTo += 1;
			}
		}
	}

	public string GetStartNode(string characterName, int day, timePeriod time_of_day){
		string startNode = characterName + ".Day" + day + "." + time_of_day.ToString() + ".Start";
		return startNode;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void SetUpNewNPCNarrative(GameObject npc, int scriptIndex = -1){
		TextAsset npcScript = getScriptForNPC(scriptIndex);
		npc.GetComponent<Yarn.Unity.Example.NPC> ().scriptToLoad = npcScript;
		//set character name by, uh, getting the file name and parsing it?
		string[] dialogNamespace;
		char[] charSeparators = new char[] {'.'};
		dialogNamespace = npcScript.name.Split (charSeparators, System.StringSplitOptions.None);
		string characterName = dialogNamespace [0];
		npc.GetComponent<Yarn.Unity.Example.NPC> ().characterName = characterName;
		npc.GetComponent<NPCstats>().NPCName = characterName;
		print ("setting dialog for new char " + characterName);
        //set up it's portratit
        SetUpNewNPCPortrait(npc);
	}

    public void SetUpNewNPCPortrait(GameObject npc)
    {
        string name = npc.GetComponent<NPCstats>().NPCName;
        foreach (npcPortraitObject portraitObject in NPCPortraitObjects)
        {
            if (name == portraitObject.NPCName)
            {
                npc.GetComponent<NPCstats>().DialogPortraits = portraitObject.portraits;
            }
            else
            {
                print("No portraits found for npc: " + name);
            }
        }
    }

	public TextAsset getScriptForNPC(int scriptIndex){
		//if script index is negative one, get one from the pool of npc scripts
		if (scriptIndex == -1) {
			if (NPCDialogs [scriptCount] != null) {
				scriptCount++;
				return NPCDialogs [scriptCount];
			} 
			return NPCDialogs [0];
		} else {
			return NPCDialogs [scriptIndex];
		}
	}

	public bool RunDreamForDay(int day){
		/* turning this off for now cuz it's busted af
		for (int i = 0; i < DreamSequence.Length; i++) {
			if (DreamSequence [i].dayDreamIsTriggered == day) {
				SceneManager.LoadScene (DreamSequence [i].SceneName);
				return true;
			}
		}
		*/
		return false;
	}

}

[System.Serializable] 
public class npcPortraitObject{
	public string NPCName;
	public Sprite[] portraits;
}

[System.Serializable]
public class GameDreamSequenceObject{
	public string SceneName;
	public int dayDreamIsTriggered;
	public string dreamDialogNode;
}
