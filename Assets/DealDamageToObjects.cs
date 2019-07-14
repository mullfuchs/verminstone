using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToObjects : MonoBehaviour {

    public string AffectedTag;

	public float baseDamageAmount;
	public float knockback;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        health OtherHealth = other.GetComponent<health>();
        if (other.tag == AffectedTag && OtherHealth)
        {
			float tempDMG = baseDamageAmount;
			other.attachedRigidbody.AddForce ( Vector3.Normalize( gameObject.transform.position - other.transform.position ) * 2 );
			//is this npc holding something?
			GameObject handObj = null;

			if (gameObject.transform.parent.GetComponent<NPCInventory> () != null) {
				handObj = gameObject.transform.parent.GetComponent<NPCInventory>().getHandObject();
			}

			//if it is is it a weapon?
			if (handObj != null && handObj.GetComponent<WeaponController> () != null) {
				tempDMG += handObj.GetComponent<WeaponController> ().damage;
			}

			OtherHealth.AddDamage(tempDMG);
        }
    }

	public void increaseHitDamage(int amount){
		baseDamageAmount += amount;
		print ("damage increased to " + baseDamageAmount);
	}
}
