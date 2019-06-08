using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObjectsWithinRadius : MonoBehaviour {

    public float RessurectionPowerAmount = 5.0f;
    private health PlayerHealth;
    private PowerObject PlayerPowerObject;

	// Use this for initialization
	void Start () {
        PlayerHealth = GameObject.Find("Player").GetComponent<health>();
        PlayerPowerObject = GameObject.Find("Player").GetComponent<PowerObject>();
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
                PlayerPowerObject.RemovePowerAmount(0.1f);
            }
        }

		if (other.tag == "RagDoll" && PlayerPowerObject.getPowerAmount() >= RessurectionPowerAmount) {
			RessurectNPC (other.gameObject);
            PlayerPowerObject.RemovePowerAmount(RessurectionPowerAmount);
		}

    }

	void RessurectNPC(GameObject npc){
        //GameObject npcBody = Instantiate (npc.GetComponent<RagdollController> ().NPCCopy, npc.transform.position, Quaternion.identity);
        print("bringing npc back to life");
		GameObject npcBody = npc.transform.root.GetComponent<RagdollController>().NPCCopy;
        npcBody.SetActive(true);
        npcBody.transform.parent = null;
        npcBody.GetComponent<health>().AddHealth(40.0f);
        npcBody.transform.position = npc.transform.position;
		npcBody.GetComponent<AIStateMachine> ().handleRessurection();
		Destroy (npc.transform.root.gameObject);
	}

	//oh man what if you can heal, which takes health out of you, but you can suck the health out of npcs to get more vstone power? yeah
}
