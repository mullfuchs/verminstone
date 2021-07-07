using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetNPCEscapeFlags : MonoBehaviour {

    public List<string> npcsThatCanEscape;

    public bool hasPlayerStartedEscape = false;

    public int metersOfTunnelDug;
    public int daysLeftToEscape;
    public bool canPlayerAttemptEscape = false;
    public int metersNeededToEscape;

    public string EscapeScene;

    public GameObject EscapeReturnGameObject;
    public GameObject EntryWarpZone;

    // Use this for initialization
    void Start() {
        //check if escape ncs are zero
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        bool npcsEscaping = false;
        foreach (GameObject g in npcs)
        {
            if (g.GetComponent<NPCOverworldController>().isEscaping && !npcsThatCanEscape.Contains(g.name))
            {
                npcsEscaping = true;
                npcsThatCanEscape.Add(g.name);
            }
        }
        //if zero find a random npc
        if (!npcsEscaping)
        {
            GameObject randNPC = npcs[Random.Range(0, npcs.Length - 1)];
            npcsThatCanEscape.Add( randNPC.name);
        }

        foreach (string name in npcsThatCanEscape)
        {
            GameObject n = GameObject.Find(name);
            n.GetComponent<NPCOverworldController>().isEscaping = true;
        }


    }

    void Awake() {

    }

    // Update is called once per frame
    void Update() {

    }

    [Yarn.Unity.YarnCommand("addNPCToEscape")]
    public void addEscapingNPC(string npcName){
        //are they added already?
        if (!npcsThatCanEscape.Contains(npcName))
        {
            npcsThatCanEscape.Add(npcName);
        }

        //maybe set a variable that is specific to the npc name so we don't have to ask again
    }

    [Yarn.Unity.YarnCommand("setPlayerStartEscapeFlag")]
    public void setPlayerStartEscapeFlag()
    {
        hasPlayerStartedEscape = true;

        EscapeReturnGameObject.GetComponent<SetDialogNodeForNPCOnEnter>().enabled = false;
        
        GameObject dialogInstance = GameObject.Find("Dialogue");
        ExampleVariableStorage.DefaultVariable[] vars = dialogInstance.GetComponent<ExampleVariableStorage>().defaultVariables;
        foreach(ExampleVariableStorage.DefaultVariable x in vars)
        {
            if(x.name == "escape_active")
            {
                x.value = "true";
            }
        }
        
        
    }

    public void WarpNPCsToBed()
    {
        //I am not fucking having these bastards path back to the warp then back to bed. no

        //set target to bed
        foreach (string name in npcsThatCanEscape)
        {
            GameObject n = GameObject.Find(name);
            if(n != null)
            {
                //clearing targets makes them follow 
                //n.GetComponent<AIStateMachine>().targets.Clear();
                
                //n.GetComponent<NPCOverworldController>().isEscaping = false;
                n.GetComponent<NPCOverworldController>().SendNPCToBed();
                EntryWarpZone.GetComponent<WarpPlayerAndNPCsToZoneOnEnter>().WarpNPCToZone(n);
            }
        }
        
    }

    public void LoadEscapeScene()
    {
        SceneManager.LoadScene(EscapeScene);
    }

     

}
