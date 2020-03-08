using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDialogNodeForNPCOnEnter : MonoBehaviour {
    public string NodeToSet;
	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WorkerNPC")
        {
            other.gameObject.GetComponent<Yarn.Unity.Example.NPC>().talkToNode = NodeToSet;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
