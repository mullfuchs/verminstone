﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToObjects : MonoBehaviour {

    public string[] AffectedTags;

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
        if(other.GetComponent<health>() != null)
        {
            health OtherHealth = other.GetComponent<health>();
            if (isTagAnAffectedTag(other.tag) && OtherHealth)
            {
                float tempDMG = baseDamageAmount + AttackStat;
                if (other.attachedRigidbody != null)
                {
                    other.attachedRigidbody.AddForce(Vector3.Normalize(gameObject.transform.position - other.transform.position) * 2);
                }
                //is this npc holding something?
                GameObject handObj = null;

                if (gameObject.transform.parent.GetComponent<NPCInventory>() != null)
                {
                    handObj = gameObject.transform.parent.GetComponent<NPCInventory>().getHandObject();
                }

                //if it is, is it a weapon?
                if (handObj != null && handObj.GetComponent<WeaponController>() != null)
                {
                    tempDMG += handObj.GetComponent<WeaponController>().damage;
                }

                OtherHealth.AddDamage(tempDMG);
                //print (gameObject.transform.parent.name + " Doing damage to " + other.gameObject.transform.parent.name);

                if (hitEffect != null)
                {
                    Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
                }
            }
        }
        else
        {
            if (other.GetComponent<VStoneObject>() != null)
            {
                VStoneObject vstone = other.GetComponent<VStoneObject>();
                float tempDMG = baseDamageAmount + AttackStat;
                GameObject handObj = null;

                if (gameObject.transform.parent.GetComponent<NPCInventory>() != null)
                {
                    handObj = gameObject.transform.parent.GetComponent<NPCInventory>().getHandObject();
                }

                //if it is, is it a weapon?
                if (handObj != null && handObj.GetComponent<WeaponController>() != null)
                {
                    tempDMG += handObj.GetComponent<WeaponController>().damage;
                }

                vstone.DamageStone(tempDMG);
            }
        }
        
    }

	public void increaseHitDamage(int amount){
		baseDamageAmount += amount;
		print ("damage increased to " + baseDamageAmount);
	}

    private bool isTagAnAffectedTag(string tag)
    {
        foreach(string t in AffectedTags)
        {
            if(tag == t)
            {
                return true;
            }
        }
        return false;
    }
}
