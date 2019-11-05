using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToObjects : MonoBehaviour {

    public string AffectedTag;

	public float baseDamageAmount;
	public float knockback;
	public GameObject hitEffect;

    private int AttackStat = 0;

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        if(parent.GetComponent<NPCstats>() != null)
        {
            AttackStat = parent.GetComponent<NPCstats>().attack;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        health OtherHealth = other.GetComponent<health>();
        if (other.tag == AffectedTag && OtherHealth)
        {
			float tempDMG = baseDamageAmount + AttackStat;
			if (other.attachedRigidbody != null) {
				other.attachedRigidbody.AddForce ( Vector3.Normalize( gameObject.transform.position - other.transform.position ) * 2 );
			}
			//is this npc holding something?
			GameObject handObj = null;

			if (gameObject.transform.parent.GetComponent<NPCInventory> () != null) {
				handObj = gameObject.transform.parent.GetComponent<NPCInventory>().getHandObject();
			}

			//if it is, is it a weapon?
			if (handObj != null && handObj.GetComponent<WeaponController> () != null) {
				tempDMG += handObj.GetComponent<WeaponController> ().damage;
			}

			OtherHealth.AddDamage(tempDMG);
			//print (gameObject.transform.parent.name + " Doing damage to " + other.gameObject.transform.parent.name);

			if (hitEffect != null) {
				Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
			}
        }
    }

	public void increaseHitDamage(int amount){
		baseDamageAmount += amount;
		print ("damage increased to " + baseDamageAmount);
	}
}
