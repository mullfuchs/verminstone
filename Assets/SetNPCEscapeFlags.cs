using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNPCEscapeFlags : MonoBehaviour {

    public List<string> npcsThatCanEscape;

    public bool hasPlayerStartedEscape = false;

    public int metersOfTunnelDug;
    public int daysLeftToEscape;
    public bool canPlayerAttemptEscape = false;
    public int metersNeededToEscape;

    // Use this for initialization
    void Start() {
        //check if escape ncs are zero
        //if zero find a random npc
        if(npcsThatCanEscape.Count == 0)
        {
            GameObject[] npcs = GameObject.FindGameObjectsWithTag("WorkerNPC");
            GameObject randNPC = npcs[Random.Range(0, npcs.Length - 1)];
            npcsThatCanEscape.Add( randNPC.name);

            foreach (string name in npcsThatCanEscape)
            {
                GameObject n = GameObject.Find(name);
                n.GetComponent<NPCOverworldController>().isEscaping = true;
            }


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

     

}
