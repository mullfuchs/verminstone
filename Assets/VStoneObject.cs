﻿using UnityEngine;
using System.Collections;

public class VStoneObject : MonoBehaviour {

	public bool HasBeenTouched = false;
	public bool HasBeenMined = false;
	public bool IsBeingMined = false;
    private bool SentForHelp = false;
	private int minersInRadius = 0;
	public int healthPoints = 500;
	public float energy = 0.5f;
	private float timer = 1.0f;
	private float timerOGval = 0;
	public GameObject VStoneFragmentObject;
    private GameObject playerObject;
    private GameObject EnemyTeamHandler;
	public int FragmentsToMake = 5;

	// Use this for initialization
	void Start () {
		timerOGval = timer;
        playerObject = GameObject.Find("Player");
        EnemyTeamHandler = GameObject.Find("EnemyNPCHandler");
	}
	
	// Update is called once per frame
	void Update () {
		if (IsBeingMined) {
			timer -= Time.deltaTime;
			if (timer <= 0.0f) {
				timer = timerOGval;
				MineStone ();
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" && !HasBeenTouched) {
			HasBeenTouched = true;
			this.GetComponent<ParticleSystem>().Stop();
		}

		if (other.tag == "WorkerNPC" && HasBeenTouched) {
			print ("stone being mined");
			if (IsBeingMined == false) {
				IsBeingMined = true;
			}
			minersInRadius++;

            if(SentForHelp == false)
            {
                EnemyTeamHandler.GetComponent<EnemyTeamHandler>().sendSwarmToAttackWhenVStoneMined(gameObject);
                SentForHelp = true;
            }
		}

//		if (other.tag == "Miner" && HasBeenTouched) {
//			HasBeenMined = true;
//		}
//
//		if (other.tag == "Carrier" && HasBeenMined) {
//			this.GetComponent<SphereCollider>().enabled = false;
//		}

	}
	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {
			energy = 0;
		}

		if (other.tag == "NPCWorker") {
			minersInRadius--;
			if (minersInRadius <= 0) {
				IsBeingMined = false;
			}
		}
	}

	void MineStone(){
		if (minersInRadius >= 1) {
			healthPoints -= minersInRadius * 10;
			if (healthPoints <= 0) {
				DestroyStoneAndCreateRocksToPickUp ();		
			}
		}
	}

	void DestroyStoneAndCreateRocksToPickUp(){
		float offsetX = 0.0f;
		for (int i = 0; i < FragmentsToMake; i++) {
			Vector3 newPosition = gameObject.transform.position + new Vector3(0 + offsetX, 0, 0);
			GameObject fragment = Instantiate (VStoneFragmentObject, newPosition, Quaternion.identity);
			fragment.GetComponent<Rigidbody> ().AddForce (new Vector3 (Random.Range(1.0f, 5.0f) , 0.5f, Random.Range(1.0f, 5.0f) ), ForceMode.Impulse);
			playerObject.GetComponent<NPCTeamHandler>().AddTargetForNPCs( fragment );
			offsetX += 1.5f;
		}
		//try to remove it from the list
		if (GameObject.Find ("CaveManager") != null) {
			GameObject.Find("CaveManager").GetComponent<CaveManager>().RemoveObjectFromFloor(gameObject);
		}
		Destroy(gameObject);
	}
}
