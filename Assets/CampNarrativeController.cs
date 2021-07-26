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

    private bool hasDialogBeenLoaded = false;

    private GameObject playerObjectReference;
    private GameObject[] npcRefernces;

	// Use this for initialization
	void Start () {
	    //get the list of npcs
		day = 1;
		timeOfDay = timePeriod.Morning;

		SetUpNarratives();
		SetUpNPCPortraits ();
        playerObjectReference = GameObject.Find("Player");
        npcRefernces = GameObject.FindGameObjectsWithTag("WorkerNPC");
    }

	public void SetUpNarratives(){
		GameObject[] npcs;
        GameObject[] NonWorkers;
		npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
        NonWorkers = GameObject.FindGameObjectsWithTag("dialog_npc");

        GameObject[] combinedNPCs = new GameObject[npcs.Length + NonWorkers.Length];
        System.Array.Copy(npcs, combinedNPCs, npcs.Length);
        System.Array.Copy(NonWorkers, 0, combinedNPCs, npcs.Length, NonWorkers.Length);

        foreach (GameObject npc in combinedNPCs)
        {
            string npcName = npc.GetComponent<NPCstats>().NPCName;

            foreach(TextAsset text in NPCDialogs)
            {
                if(npcName + ".Main" == text.name) //look for the name of the script to load using name of NPC
                {
                    npc.GetComponent<Yarn.Unity.Example.NPC>().scriptToLoad = text;
                    npc.GetComponent<Yarn.Unity.Example.NPC>().characterName = npcName;
                    npc.GetComponent<NPCstats>().NPCScriptIndex = 0;
                }
            }

            if(npc.GetComponent<Yarn.Unity.Example.NPC>().scriptToLoad == null)
            {
                print("couldn't load dialog for " + npcName);
            }
        }

        //put in dialog for workerNPCs too


        /*
		for(int i = 0; i < npcs.Length; i++){ //well this was stupid!


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
			//print ("setting dialog for char " + characterName);
		}
        */
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
			//print ("start node" + startnode);
			npc.GetComponent<Yarn.Unity.Example.NPC> ().talkToNode = startnode;
		}

        GameObject[] NonWorkernpcs;
        NonWorkernpcs = GameObject.FindGameObjectsWithTag("dialog_npc");
        foreach (GameObject npc in NonWorkernpcs)
        {
            string startnode = GetStartNode(npc.GetComponent<NPCstats>().NPCName, npc.GetComponent<NPCstats>().daysTalkedTo, timeOfDay);
            //print ("start node" + startnode);
            npc.GetComponent<Yarn.Unity.Example.NPC>().talkToNode = startnode;
        }
        //GameObject.Find ("Dialogue").GetComponent<Yarn.Unity.DialogueRunner> ().HotLoadNPCScripts ();
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

	public void AdvanceDialogDayOfKeyNPCs(){
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
		TextAsset npcScript = getScriptForNPC(npc.GetComponent<NPCstats>().NPCName);
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
                //print("No portraits found for npc: " + name);
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

    public TextAsset getScriptForNPC(string name)
    {
        TextAsset returnDialog = null;

        foreach (TextAsset text in NPCDialogs)
        {
            if (name + ".Main" == text.name) //look for the name of the script to load using name of NPC
            {
                returnDialog = text;
            }
        }

        if (returnDialog == null)
        {
            print("couldn't load dialog for " + name);
        }

        return returnDialog;
    }

    public Sprite[] getPotraitsForKeyNPC(string npcName)
    {
        foreach(npcPortraitObject portraitObject in KeyNPCPortraitObjects)
        {
            if(npcName == portraitObject.NPCName)
            {
                return portraitObject.portraits;
            }
        }

        return null;
    }

	public bool RunDreamForDay(int day){
        // turning this back on for now cuz it's busted af
        
        for (int i = 0; i < DreamSequence.Length; i++) {
			if (DreamSequence [i].dayDreamIsTriggered == day) {
                SetPlayerAndNPCsActive(false);
				SceneManager.LoadScene (DreamSequence [i].SceneName);
				return true;
			}
		}


		return false;
	}

    public void SetPlayerAndNPCsActive(bool areActive)
    {
        playerObjectReference.SetActive(areActive);
        /*
        foreach (GameObject npc in npcRefernces)
        {
            npc.SetActive(areActive);
        }
        */
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
    public Scene SceneFile;
	public int dayDreamIsTriggered;
	public string dreamDialogNode;
}
