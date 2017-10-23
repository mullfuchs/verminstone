using UnityEngine;
using System.Collections;

public class VStoneObject : MonoBehaviour {

	public bool HasBeenTouched = false;
	public bool HasBeenMined = false;
	public bool IsBeingMined = false;
	private int minersInRadius = 0;
	public int healthPoints = 500;
	public int energy = 10;
	private float timer = 1.0f;
	private float timerOGval = 0;
	public GameObject VStoneFragmentObject;
	public int FragmentsToMake = 5;

	// Use this for initialization
	void Start () {
		timerOGval = timer;
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
		healthPoints -= minersInRadius * 10;
		if (healthPoints <= 0) {
			DestroyStoneAndCreateRocksToPickUp ();		
		}
	}

	void DestroyStoneAndCreateRocksToPickUp(){
		for (int i = 0; i < FragmentsToMake; i++) {
			Instantiate (VStoneFragmentObject, gameObject.transform.position, Quaternion.identity);
		}
		Destroy(gameObject);
	}
}
