using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObjectsWithinRadius : MonoBehaviour {

    private health PlayerHealth;

	// Use this for initialization
	void Start () {
        PlayerHealth = GameObject.Find("Player").GetComponent<health>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        health objectHealth = other.gameObject.GetComponent<health>();
        if(other.tag == "WorkerNPC" && objectHealth != null)
        {
            if (objectHealth.AddHealth(1.0f))
            {
                PlayerHealth.AddDamage(0.3f);
            }
        }

		if (other.name == "WorkerNPCRagdoll" && objectHealth != null) {
			RessurectNPC (other.gameObject);
		}

    }

	void RessurectNPC(GameObject npc){
		GameObject.Instantiate (npc.GetComponent<RagdollController> ().NPCCopy, npc.transform.position, Quaternion.identity);
		Destroy (npc);
		//ragdoll should have a reference to the npc
		//spawn it in the same position as the ragdoll
		//destroy the ragdoll
	}

	//oh man what if you can heal, which takes health out of you, but you can suck the health out of npcs to get more vstone power? yeah
}
