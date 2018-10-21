using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public int damage;

	// Use this for initialization
	void Start () {
		//get parent

		//find 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateNPCHitboxDamage(){
		Transform hitbox = gameObject.transform.parent.Find("AttackHitBox");

		if (hitbox != null) {
			hitbox.GetComponent<DealDamageToObjects> ().increaseHitDamage (damage);
		} else {
			print ("can't find hitbox");
		}
		hitbox.gameObject.SetActive (false);
	}
}
