using UnityEngine;
using System.Collections;
using UnityStandardAssets;

public class NPCTeamHandler : MonoBehaviour {

	public GameObject[] NPCMiners = new GameObject[5];
	public GameObject[] NPCCarriers = new GameObject[5];

	Queue ActiveStones = new Queue();
	Queue MinedStones = new Queue();

	Queue MinerQueue = new Queue();
	Queue CarrierQueue = new Queue();

	// Use this for initialization
	void Start () {
		NPCMiners = GameObject.FindGameObjectsWithTag ("Miner");
		NPCCarriers = GameObject.FindGameObjectsWithTag ("Carrier");
		foreach (GameObject g in NPCMiners) {
			print ("added miner");
			MinerQueue.Enqueue(g);
		}
		foreach (GameObject g in NPCCarriers) {
			print ("added carrier");
			CarrierQueue.Enqueue(g);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (ActiveStones.Count > 0 && MinerQueue.Count > 0) {
			print ("Sending miner to mine rock");
			SendNPCToMineRock((GameObject)MinerQueue.Dequeue(), (GameObject)ActiveStones.Dequeue());
		}
		
		if (MinedStones.Count > 0 && CarrierQueue.Count > 0) {
			print ("Sending carrier to pick up rock");
			SendNPCToPickUpRock((GameObject)CarrierQueue.Dequeue(), (GameObject)MinedStones.Dequeue());
		}
	}

	void CheckToSeeIfARockCanBePickedUpOrMined(){

	}

	public void AddStoneToBeMined(GameObject rock){
		ActiveStones.Enqueue (rock);
		CheckToSeeIfARockCanBePickedUpOrMined ();
	}

	public void AddStoneToBePickedUp(GameObject rock){
		MinedStones.Enqueue (rock);
		CheckToSeeIfARockCanBePickedUpOrMined ();
	}

	public void AddNPCToMinerQueue(GameObject NPC){
		MinerQueue.Enqueue (NPC);
		CheckToSeeIfARockCanBePickedUpOrMined ();
	}

	public void AddNPCToCarrierQueue(GameObject NPC){
		CarrierQueue.Enqueue (NPC);
		CheckToSeeIfARockCanBePickedUpOrMined ();
	}

	public void MineVerminStone(GameObject rock){
		AddStoneToBeMined (rock);
	}

	public void SendNPCToMineRock(GameObject NPCFollower, GameObject rock){
		NPCFollower.GetComponent<AIStateMachine>().GetStone(rock);
	}

	public void SendNPCToPickUpRock(GameObject NPCCarrier, GameObject rock){
		NPCCarrier.GetComponent<AIStateMachine> ().GetStone (rock);
	}


}
