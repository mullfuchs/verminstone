using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPromptOnEnter : MonoBehaviour {

    public GameObject PromptObject;

    public bool canTalkToNPC = true;

	// Use this for initialization
	void Start () {
        if (PromptObject.activeInHierarchy)
        {
            PromptObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && canTalkToNPC)
        {
			print ("prompt object active");
            PromptObject.SetActive(true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        PromptObject.SetActive(false);
    }
}
