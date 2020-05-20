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

    }

    void Awake() {
        foreach (string name in npcsThatCanEscape) {
            GameObject n = GameObject.Find(name);
            n.GetComponent<NPCOverworldController>().isEscaping = true;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    [Yarn.Unity.YarnCommand("addNPCToEscape")]
    public void addEscapingNPC(string npcName){
        npcsThatCanEscape.Add(npcName);
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
                x.value = "1";
            }
        }
        
    }



}
