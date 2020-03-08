using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetForNPCOnEnter : MonoBehaviour {

    public GameObject target;

    // Use this for initialization
    void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WorkerNPC")
        {
            other.GetComponent<AIStateMachine>().AddTargetForNPC(target);
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
